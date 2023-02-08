using System;
using System.Collections.Generic;
using System.Linq;
using AI.GPUFlock;
using Datastructures;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Stats;
using UnityEngine;

namespace Army
{

	public class ArmyMoverFlock : MonoBehaviour, IArmyMover, IUnitGroupSerializer
	{
		[SerializeField] private Transform _targetTransform;
		private Tracked<Vector2> _target = new Tracked<Vector2>(Vector2.zero);
		private GPUUnitDraw[] _units;

		private Vector2 _center;
		private Vector2 _offset;

		private ArmyMoverArgs _args;


		public void Init(ArmyMoverArgs args)
		{
			_args = args;
		}
		private void Start()
		{
			FlocksHandler.Instance.AddUnitBufferHandler(this);
		}


		public GPUUnitDraw[] Serialize()
		{
			_units = new GPUUnitDraw[_args.Units.List.Count];
			for(int i = 0; i < _args.Units.List.Count; i++)
			{
				_units[i].Position = _args.Units.List[i].Position;
				_units[i].Direction = _args.Units.List[i].Direction;
				_units[i].TargetPos = _target.Value;
				_units[i].Noise_Offset = 1f;
				_units[i].Team = _args.Team;
			}
			return _units;
		}

		public void Deserialize(GPUUnitDraw[] buffer)
		{
			_units = buffer;
			for(int i = 0; i < buffer.Length; i++)
			{
				_args.Units.List[i].Position = _units[i].Position;
				_args.Units.List[i].Direction = _units[i].Direction;
				_args.Units.List[i].TargetIndex = _units[i].TargetedUnit;
			}
		}


		public void SetTarget(Vector2 dir)
		{
			SetTargetMove(dir);
			_targetTransform.position = new Vector3(_target.Value.x, 1, _target.Value.y);
		}
		private void SetTargetOffset(Vector2 dir)
		{
			_center = _args.Units.List.Select(x => x.Position).Aggregate((x, y) => x + y) / _args.Units.List.Count;
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