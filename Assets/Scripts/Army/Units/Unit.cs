using System;
using Army.Units.UnitEventArgs;
using DefaultNamespace.Enums;
using GameState;
using Graphics;
using Stats;
using UnityEngine;

namespace Army.Units
{
	public class Unit
	{
		public EventHandler<UnitKilledEventArgs> OnUnitKilled;
		public EventHandler<UnitDamagedEventArgs> OnUnitDamaged;
		public EventHandler<UnitAttackedEventArgs> OnUnitAttackPerformed;

		private ArmyKDTree _armyKDTree;

		public IUnitAnimator Animator;
		private Vector2 _target;
		public StatDict<ArmyStat> Stats{get; private set;}
		public Vector2 Position{get; set;}
		public float Health{get; private set;}
		public Vector2 Direction{get; set;}
		public int TargetIndex{get; set;} = -1;
		public uint Team{get; set;}
		public int XScale{get; set;} = 1;
		public Vector2 TargetPos{get; set;}

		private float _timeSinceLastAttack = 0;

		public Unit(float health, Vector2 position, ArmyKDTree armyKDTree, StatDict<ArmyStat> stats, uint team, IUnitAnimator animator)
		{
			Health = health;
			Position = position;
			_armyKDTree = armyKDTree;
			Stats = stats;
			Team = team;
			Animator = animator;
			Ticker.OnTick += OnTick;
		}
		~Unit()
		{
			Ticker.OnTick -= OnTick;
		}

		private void OnTick(Ticker.OnTickEventArgs obj)
		{
			_timeSinceLastAttack += obj.DeltaTime;
			if(_target != Vector2.zero && (_target - Position).sqrMagnitude > 0.01f)
			{
				MoveWithDir(_target - Position);
			}
			if(CanAttack())
			{
				var target = AllUnits.Units[TargetIndex];
				target.Damage(Stats[ArmyStat.Damage]);
				OnUnitAttackPerformed?.Invoke(this, new UnitAttackedEventArgs() { Attacker = this, Target = target });
				//target.Position += (target.Position - Position).normalized * Stats[ArmyStat.Damage] * 0.1f;
				_timeSinceLastAttack = 0;
			}
			if(Health <= 0)
			{
				Kill();
			}
		}
		private bool CanAttack()
		{
			return (_timeSinceLastAttack > 1 / Stats[ArmyStat.AttackSpeed]) && TargetIndex != -1;
		}
		public void Damage(float damage)
		{
			Health -= damage;
			OnUnitDamaged?.Invoke(this, new UnitDamagedEventArgs() { Target = this, Damage = damage });
		}

		public void Kill()
		{
			OnUnitKilled?.Invoke(this, new UnitKilledEventArgs() { Target = this });
			Ticker.OnTick -= OnTick;
		}

		public void MoveWithDir(Vector2 offset)
		{
			Position += offset * Time.deltaTime;
		}
		public void SetTarget(Vector2 target)
		{
			_target = target;
		}


	}

}