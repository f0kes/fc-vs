using System.Linq;
using Datastructures;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Army
{
	public class ArmyMover : MonoBehaviour, IArmyMover
	{
		private Vector2 _velocity;
		private Tracked<Vector2> _target = new Tracked<Vector2>(Vector2.zero);
		private Vector2 _center;
		private Vector2 _offset;
		private  ArmyMoverArgs _armyMoverArgs;

		public void Init(ArmyMoverArgs args)
		{
			_armyMoverArgs = args;
		}

		private void Update()
		{
			Move();
		}
		
		public void SetTarget(Vector2 dir)
		{
			_center = _armyMoverArgs.Units.Units.Select(x => x.Position).Aggregate((x, y) => x + y) / _armyMoverArgs.Units.Units.Count;
			_offset = dir * _armyMoverArgs.ArmyRadius * 1.1f;
			_target.Set(_center + _offset);
		}
		private void Move()
		{
			var points = _armyMoverArgs.Formation.EvaluatePoints(_armyMoverArgs.Units.Units.Count).ToArray();
			var target = _target.Value;
			if(_offset == Vector2.zero)
				return;
			for(var i = 0; i < _armyMoverArgs.Units.Units.Count; i++)
			{
				var unit = _armyMoverArgs.Units.Units[i];
				var move = points[i] + target;
				unit.SetTarget(move);
			}
		}
		public void MoveInRadius(Vector2 point)
		{
			var unitsInRadius = _armyMoverArgs.ArmyKDTree.GetUnitsInRadius(point, 3f);
			foreach(var unit in unitsInRadius)
			{
				unit.MoveWithDir((-point + unit.Position).normalized * _armyMoverArgs.Stats[ArmyStat.Speed]);
			}
		}
	}
}