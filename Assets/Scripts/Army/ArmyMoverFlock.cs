using System;
using System.Collections.Generic;
using System.Linq;
using Datastructures;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Stats;
using UnityEngine;

namespace Army
{
	public struct GPUUnitDraw
	{
		public Vector2 Position;
		public Vector2 Direction;
		public Vector2 TargetPos;
		public float Noise_Offset;
	}
	public class ArmyMoverFlock : MonoBehaviour, IArmyMover, IUnitBufferHandler
	{
		[SerializeField] private ComputeShader _computeFlock;
		[SerializeField] private float _neighborDistance = 1f;
		[SerializeField] private float _boidSpeedVariation = 1f;
		[SerializeField] private Transform _targetTransform;
		private Tracked<Vector2> _target = new Tracked<Vector2>(Vector2.zero);
		private GPUUnitDraw[] _units;
		private ComputeBuffer _unitsBuffer;
		private int _kernel;
		private Vector2 _center;
		private Vector2 _offset;

		private ArmyMoverArgs _args;


		const int THREADS = 256;

		public void Init(ArmyMoverArgs args)
		{
			_args = args;
		}
		private void Start()
		{
			_kernel = _computeFlock.FindKernel("CSMain");
		}
		private void Update()
		{
			if(_args.Units.Count == 0)
				return;
			var stride = (sizeof(float) * 2) * 3 + sizeof(float);
			_unitsBuffer = new ComputeBuffer(_args.Units.Count, stride);
			ConvertToBuffer();
			Dispatch();
			ConvertFromBuffer();
			_unitsBuffer.Release();
		}
		private void Dispatch()
		{
			_computeFlock.SetFloat("DeltaTime", Time.deltaTime);
			_computeFlock.SetFloat("RotationSpeed", _args.Stats[ArmyStat.RotationSpeed]);
			_computeFlock.SetFloat("BoidSpeed", _args.Stats[ArmyStat.Speed]);
			_computeFlock.SetFloat("BoidSpeedVariation", _boidSpeedVariation);
			//_computeFlock.SetVector("TargetPosition", _target.Value);
			_computeFlock.SetFloat("NeighbourDistance", _neighborDistance);
			_computeFlock.SetInt("BoidsCount", _args.Units.Count);
			_computeFlock.SetBuffer(_kernel, "boidBuffer", _unitsBuffer);

			_computeFlock.Dispatch(_kernel, _args.Units.Count / THREADS + 1, 1, 1);
		}

		private void ConvertToBuffer()
		{
			_units = new GPUUnitDraw[_args.Units.Count];
			for(int i = 0; i < _args.Units.Count; i++)
			{
				_units[i].Position = _args.Units[i].Position;
				_units[i].Direction = _args.Units[i].Direction;
				_units[i].TargetPos = _target.Value;
				_units[i].Noise_Offset = 1f;
			}
			_unitsBuffer.SetData(_units);
		}
		private void ConvertFromBuffer()
		{
			_unitsBuffer.GetData(_units);
			for(int i = 0; i < _args.Units.Count; i++)
			{
				_args.Units[i].Position = _units[i].Position;
				_args.Units[i].Direction = _units[i].Direction;
			}
		}
		public GPUUnitDraw[] GetBuffer()
		{
			_units = new GPUUnitDraw[_args.Units.Count];
			for(int i = 0; i < _args.Units.Count; i++)
			{
				_units[i].Position = _args.Units[i].Position;
				_units[i].Direction = _args.Units[i].Direction;
				_units[i].TargetPos = _target.Value;
				_units[i].Noise_Offset = 1f;
			}
			return _units;
		}

		public void SetBuffer(GPUUnitDraw[] buffer)
		{
			_units = buffer;
			for(int i = 0; i < _args.Units.Count; i++)
			{
				_args.Units[i].Position = _units[i].Position;
				_args.Units[i].Direction = _units[i].Direction;
			}
		}


		public void SetTarget(Vector2 dir)
		{
			SetTargetMove(dir);
			_targetTransform.position = new Vector3(_target.Value.x, 1, _target.Value.y);
		}
		private void SetTargetOffset(Vector2 dir)
		{
			_center = _args.Units.Select(x => x.Position).Aggregate((x, y) => x + y) / _args.Units.Count;
			_offset = dir * _args.ArmyRadius * 1.1f;
			_target.Set(_center + _offset);
		}
		private void SetTargetMove(Vector2 dir)
		{
			Vector2 offset = dir * _args.Stats[ArmyStat.Speed] * Time.deltaTime * 1.5f;
			_target.Set(_target.Value + offset);
		}

		public void MoveInRadius(Vector2 point)
		{
			var unitsInRadius = _args.ArmyKDTree.GetNearestUnitsRadius(point, 3f);
			foreach(var unit in unitsInRadius)
			{
				//unit.MoveWithDir((-point + unit.Position).normalized * _args.Stats[ArmyStat.Speed]);
				unit.Kill();
			}
		}

		
	}
	
}