using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace ArmyInput
{
	public class AutoInputHandler : MonoBehaviour, IInputHandler
	{
		[SerializeField] private Army.Army _controlledArmy;
		[SerializeField] private float _spottingDistance = 30;
		private Army.Army _currentTarget;
		private bool _hasTarget;
		private float SpottingDistanceSqr => _spottingDistance * _spottingDistance;
		public InputEvents InputEvents{get;} = new InputEvents();
		private void Update()
		{
			var move = new Vector2();
			var avoid = new Vector2(0, 0);
			if(!_hasTarget || _currentTarget == null)
			{
				_currentTarget = GetClosestEnemy();
				if(_currentTarget != null)
				{
					_hasTarget = true;
				}
			}
			else
			{
				if(SqrDistanceToTarget() <= SpottingDistanceSqr)
				{
					var dir = _currentTarget.Leader.transform.position - _controlledArmy.Leader.transform.position;
					var dir2D = new Vector2(dir.x, dir.z);
					move += dir2D;
				}
				else
				{
					_hasTarget = false;
				}
			}
			InputEvents.OnMove?.Invoke(move.NormalizeIfLong());
		}
		private float SqrDistanceToTarget()
		{
			return Vector3.SqrMagnitude(_controlledArmy.Leader.transform.position - _currentTarget.Leader.transform.position);
		}
		private List<Army.Army> GetAllyArmiesInSight()
		{
			return Army.Army.Armies
				.Where(army => army.Team == _controlledArmy.Team)
				.Where(army => Vector3.SqrMagnitude(army.Leader.transform.position - _controlledArmy.Leader.transform.position) <= SpottingDistanceSqr)
				.ToList();
		}
		private Army.Army GetClosestEnemy()
		{
			return Army.Army.Armies
				.Where(army => army.Team != _controlledArmy.Team)
				.OrderBy(army => Vector3.SqrMagnitude(army.Leader.transform.position - _controlledArmy.Leader.transform.position))
				.FirstOrDefault();
		}
	}

}