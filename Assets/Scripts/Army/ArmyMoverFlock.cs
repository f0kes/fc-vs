using System;
using System.Collections.Generic;
using System.Linq;
using AI.GPUFlock;
using Datastructures;
using DefaultNamespace.Enums;
using Formations.Scripts;
using GameState;
using Stats;
using UnityEngine;

namespace Army
{

	public class ArmyMoverFlock : MonoBehaviour, IArmyMover, ITickable
	{
		[SerializeField] private Transform _targetTransform;
		private Tracked<Vector2> _target = new Tracked<Vector2>(Vector2.zero);
		private Vector3 _initialTargetPos;

		private Vector2 _center;
		private Vector2 _offset;

		private ArmyMoverArgs _args;


		public void Init(ArmyMoverArgs args)
		{
			_args = args;
			Ticker.AddTickable(this);
			var position = _targetTransform.position;
			_target.Set(new Vector2(position.x, position.z));
			_initialTargetPos = position;
		}

		public void OnTick(Ticker.OnTickEventArgs obj)
		{
			foreach(var unit in _args.Units.Units)
			{
				unit.TargetPos = _target.Value;
			}
		}
		public void SetTarget(Vector2 dir)
		{
			SetTargetMove(dir);
			_targetTransform.position = new Vector3(_target.Value.x, 0, _target.Value.y + _initialTargetPos.y);
		}
		private void SetTargetMove(Vector2 dir)
		{
			Vector2 offset = dir * _args.Stats[ArmyStat.Speed] * Time.deltaTime;
			_target.Set(_target.Value + offset);
		}

		public void MoveInRadius(Vector2 point)
		{
			var unitsInRadius = _args.ArmyKDTree.GetUnitsInRadius(point, 3f);
			foreach(var unit in unitsInRadius)
			{
				//unit.MoveWithDir((-point + unit.Position).normalized * _args.Stats[ArmyStat.Speed]);
				unit.Kill();
			}
		}
	}
}