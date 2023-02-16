using System;
using Army.Units.UnitEventArgs;
using DefaultNamespace.Enums;
using GameState;
using Graphics;
using Stats;
using UnityEngine;

namespace Army.Units
{
	public class Unit : ITickable
	{
		public EventHandler<UnitKilledEventArgs> OnUnitKilled;
		public EventHandler<UnitDamagedEventArgs> OnUnitDamaged;
		public EventHandler<UnitAttackedEventArgs> OnUnitAttackPerformed;



		public IUnitAnimator Animator;
		private Vector2 _target;

		public UnitGroup Group{get; set;}
		public StatDict<ArmyStat> Stats{get; private set;}
		public Vector2 Position{get; set;}
		public float Health{get; private set;}
		public Vector2 Direction{get; set;}
		public int TargetIndex{get; set;} = -1;

		public uint UnitGroupIndex{get; set;}

		public int XScale{get; set;} = 1;
		public Vector2 TargetPos{get; set;}

		private float _timeSinceLastAttack = 0;
		private Vector2 _pushForce = Vector2.zero;

		public Unit(float health, Vector2 position, StatDict<ArmyStat> stats, IUnitAnimator animator)
		{
			Health = health;
			Position = position;
			Stats = stats;

			Animator = animator;
			Ticker.AddTickable(this);
		}
		~Unit()
		{
			//Ticker.OnTick -= OnTick;
		}
		//TODO: to compute shader
		public void OnTick(Ticker.OnTickEventArgs obj)
		{
			_timeSinceLastAttack += obj.DeltaTime;
			if(_pushForce.sqrMagnitude > 0.01f)
			{
				MoveWithDir(_pushForce);
				_pushForce *= 0.9f;
			}
			if(CanAttack())
			{
				var target = AllUnits.Units[TargetIndex];
				target.Damage(Stats[ArmyStat.Damage]);
				target.Push((target.Position - Position).normalized * Stats[ArmyStat.Damage] * 2f);
				OnUnitAttackPerformed?.Invoke(this, new UnitAttackedEventArgs() { Attacker = this, Target = target });
				//target.Position += (target.Position - Position).normalized * Stats[ArmyStat.Damage] * 0.1f;
				_timeSinceLastAttack = 0;
			}
			if(Health <= 0)
			{
				Kill();
			}
		}
		//TODO: to compute shader
		private bool CanAttack()
		{
			return (_timeSinceLastAttack > 1 / Stats[ArmyStat.AttackSpeed]) && TargetIndex != -1;
		}
		public void Damage(float damage)
		{
			Health -= damage;
			OnUnitDamaged?.Invoke(this, new UnitDamagedEventArgs() { Target = this, Damage = damage });
		}
		public void Push(Vector2 dir)
		{
			_pushForce += dir;
		}

		public void Kill()
		{
			OnUnitKilled?.Invoke(this, new UnitKilledEventArgs() { Target = this });
			Ticker.RemoveTickable(this);
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