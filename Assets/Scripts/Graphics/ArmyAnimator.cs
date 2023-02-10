using Army.Units;
using DefaultNamespace.Enums;
using Enums;
using GameState;
using UnityEngine;

namespace Graphics
{
	public class ArmyAnimator
	{
		private UnitGroup _units;
		public ArmyAnimator(UnitGroup units)
		{
			_units = units;
			Ticker.OnTick += OnTick;
		}
		~ArmyAnimator()
		{
			Ticker.OnTick -= OnTick;
		}
		private void OnTick(Ticker.OnTickEventArgs obj)
		{
			foreach(var unit in _units.Units)
			{
				if(unit.Direction.sqrMagnitude > 0.01f)
				{
					if(unit.Direction.x > 0)
						unit.XScale = 1;
					else
						unit.XScale = -1;
					unit.Animator.SetAnimation(UnitAnimationType.Run, time:  0.7f);
				}
				else
				{
					unit.Animator.SetAnimation(UnitAnimationType.Idle, time: 1f);
				}
			}
		}
	}
}