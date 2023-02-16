using System;
using Army.Units;
using Army;
using GameState;
using UnityEngine;

namespace Player
{
	public class UnitGroupSelector : ITickable
	{
		public event Action<UnitGroup> OnUnitGroupSelected;
		public event Action<UnitGroup> OnUnitGroupHovered;

		private UnitGroup _currentHoveredGroup;

		private IPointerProvider _pointerProvider;
		public UnitGroupSelector(IPointerProvider pointerProvider)
		{
			_pointerProvider = pointerProvider;
			Ticker.AddTickable(this);
		}
		~UnitGroupSelector()
		{
			Ticker.RemoveTickable(this);
		}

		public void OnTick(Ticker.OnTickEventArgs args)
		{
			var pointerPosition = _pointerProvider.GetPointerPosition();
			var hoveredGroup = Army.Army.KDTree.GetNearestUnits(pointerPosition, 1);
			if(hoveredGroup.Count == 0)
			{
				_currentHoveredGroup = null;
				return;
			}
			var hoveredUnit = hoveredGroup[0];
			var hoveredUnitGroup = hoveredUnit.Group;
			if(hoveredUnitGroup != _currentHoveredGroup)
			{
				_currentHoveredGroup = hoveredUnitGroup;
				OnUnitGroupHovered?.Invoke(_currentHoveredGroup);
			}
			if(Input.GetMouseButtonDown(0))
			{
				OnUnitGroupSelected?.Invoke(_currentHoveredGroup);
			}
		}
	}
}