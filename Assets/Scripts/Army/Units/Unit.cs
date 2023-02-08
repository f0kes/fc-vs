using System;
using DefaultNamespace.Enums;
using GameState;
using Stats;
using UnityEngine;

namespace Army.Units
{
	public class Unit
	{
		private ArmyKDTree _armyKDTree;
		public Action OnUnitKilled;
		private Vector2 _target;
		private StatDict<ArmyStat> _stats;
		public Vector2 Position{get;  set;}
		public float Health{get; private set;}
		public Vector2 Direction{get; set;}
		public int TargetIndex{get; set;} = -1;
		public uint Team{get; set;}
		public Vector2 TargetPos{get; set;}

		public Unit(float health, Vector2 position, ArmyKDTree armyKDTree, StatDict<ArmyStat> stats)
		{
			Health = health;
			Position = position;
			_armyKDTree = armyKDTree;
			_stats = stats;
			Ticker.OnTick += OnTick;
		}

		private void OnTick(Ticker.OnTickEventArgs obj)
		{
			if(_target != Vector2.zero && (_target-Position).sqrMagnitude > 0.01f)
			{
				MoveWithDir(_target - Position);
			}
		}


		public void Kill()
		{
			OnUnitKilled?.Invoke();
			Ticker.OnTick -= OnTick;
		}

		public void MoveWithDir(Vector2 offset)
		{
			Position += offset.normalized * _stats[ArmyStat.Speed] * Time.deltaTime;
		}
		public void SetTarget(Vector2 target)
		{
			_target = target;
		}
		

	}
}