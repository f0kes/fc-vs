using Army.Units;
using Army.Units.UnitEventArgs;
using DefaultNamespace.Enums;
using Enums;
using GameState;
using UnityEngine;

namespace Graphics
{
	public class ArmyAnimator: ITickable
	{
		private UnitGroup _units;
		public ArmyAnimator(UnitGroup units)
		{
			_units = units;
			Ticker.AddTickable(this);
			_units.OnUnitAdded += OnUnitAdded;
			_units.OnUnitRemoved += OnUnitRemoved;
			foreach(var unit in _units.Units)
			{
				SubscribeToUnit(unit);
			}
		}
		~ArmyAnimator()
		{
			Ticker.RemoveTickable(this);
			_units.OnUnitAdded -= OnUnitAdded;
			_units.OnUnitRemoved -= OnUnitRemoved;
			foreach(var unit in _units.Units)
			{
				UnsubscribeFromUnit(unit);
			}
		}
		private void OnUnitRemoved(Unit obj)
		{
			UnsubscribeFromUnit(obj);
		}

		private void OnUnitAdded(Unit obj)
		{
			SubscribeToUnit(obj);
		}
		private void SubscribeToUnit(Unit unit)
		{
			unit.OnUnitDamaged += OnUnitDamaged;
			unit.OnUnitAttackPerformed += OnUnitAttacked;
		}
		private void UnsubscribeFromUnit(Unit unit)
		{
			unit.OnUnitDamaged -= OnUnitDamaged;
			unit.OnUnitAttackPerformed -= OnUnitAttacked;
		}

		private void OnUnitAttacked(object sender, UnitAttackedEventArgs e)
		{
			e.Attacker.Animator.SetAnimation(UnitAnimationType.Attack, loop: false, time: 0.7f);
		}

		private void OnUnitDamaged(object sender, UnitDamagedEventArgs e)
		{
			e.Target.Animator.SetAnimation(UnitAnimationType.OnHit, loop: false, time: 0.7f);
		}



		//TODO: to compute shader
		public void OnTick(Ticker.OnTickEventArgs obj)
		{
			foreach(var unit in _units.Units)
			{
				if(unit.Direction.sqrMagnitude > 0.01f)
				{
					if(unit.Direction.x > 0)
						unit.XScale = 1;
					else
						unit.XScale = -1;
					unit.Animator.SetAnimation(UnitAnimationType.Run, time: 0.7f);
				}
				else
				{
					unit.Animator.SetAnimation(UnitAnimationType.Idle, time: 1f);
				}
			}
		}
	}
}