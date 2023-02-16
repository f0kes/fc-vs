using System;
using System.Collections.Generic;
using Army;
using Army.Units;
using DefaultNamespace.Enums;
using GameState;
using UnityEngine;
// using Army.Unit = Unit;

namespace AI.GPUFlock
{
	public struct GPUUnitDraw
	{
		public Vector2 Position;
		public Vector2 Direction;
		public Vector2 TargetPos;
		public uint GroupId;
		public int TargetedUnit;
		
		public static int GetStride()
		{
			return (sizeof(float) * 2) * 3 + sizeof(uint) + sizeof(int);
		}
	}
	public struct GPUUnitGroup
	{
		public uint Team;
		public float Speed;
		public float AttackRange;
		
		public static int GetStride()
		{
			return sizeof(uint) + sizeof(float) * 2;
		}
	}
	public class FlocksHandler : MonoBehaviour, ITickable
	{
		[SerializeField] private ComputeShader _computeFlock;
		const int THREADS = 256;
		public static  AllUnits Serializer;
		private int[] _bufferSizes;
		private int _kernel;
		private int _stride;
		private GPUUnitDraw[] _units;
		private GPUUnitGroup[] _groups;
		
		private ComputeBuffer _unitsBuffer;
		private ComputeBuffer _groupsBuffer;
		public static FlocksHandler Instance{get; private set;}
		public List<Army.Units.Unit> Units{get; private set;} = new List<Army.Units.Unit>();

		private void Awake()
		{
			if(Instance == null)
				Instance = this;
			else
			{
				Destroy(gameObject);
				return;
			}
			Ticker.AddTickable(this);
			_kernel = _computeFlock.FindKernel("CSMain");
			_stride = GPUUnitDraw.GetStride(); // 3 vectors2 + 1 float + 1 uint
		}

		public void OnTick(Ticker.OnTickEventArgs tick)
		{
			UpdateBuffers();
			Dispatch(tick.DeltaTime);
			UpdateHandlers();
			_unitsBuffer.Release();
			_groupsBuffer.Release();
		}

		private void UpdateBuffers()
		{
			_units = Serializer.GetUnits();
			_unitsBuffer = new ComputeBuffer(_units.Length, _stride);
			_unitsBuffer.SetData(_units);
			
			_groups = Serializer.GetGroups();
			_groupsBuffer = new ComputeBuffer(_groups.Length, GPUUnitGroup.GetStride());
			_groupsBuffer.SetData(_groups);
		}
		private void Dispatch(float tickTime)
		{
			_computeFlock.SetFloat("delta_time", tickTime);
			//_computeFlock.SetFloat("RotationSpeed", 1);
			//_computeFlock.SetFloat("BoidSpeed", 10);
			//_computeFlock.SetFloat("BoidSpeedVariation", 0.1f);
			//_computeFlock.SetVector("TargetPosition", _target.Value);
			_computeFlock.SetFloat("neighbour_distance", 1);
			_computeFlock.SetInt("boids_count", _units.Length);
			_computeFlock.SetBuffer(_kernel, "boid_buffer", _unitsBuffer);
			_computeFlock.SetBuffer(_kernel, "group_params_buffer", _groupsBuffer);

			_computeFlock.Dispatch(_kernel, _units.Length / THREADS + 1, 1, 1);
		}
		private void UpdateHandlers()
		{
			_unitsBuffer.GetData(_units);
			Serializer.DeserializeUnits(_units);
		}
		public void SetSerializer(AllUnits serializer)
		{
			Serializer = serializer;
		}
	}
}