using System;
using Army.Units;
using Army;
using GameState;
using UnityEngine;

namespace Player
{

	public class UnitGroupSelector : MonoBehaviour
	{
		[SerializeField] private float _selectionRadius = 2f;
		public event Action<UnitGroup> OnUnitGroupSelected;
		public event Action<UnitGroup> OnUnitGroupHovered;

		public event Action<UnitGroup> OnUnitGroupUnhovered;

		private IPointerProvider _pointerProvider;

		public IPointerProvider PointerProvider
		{
			set
			{
				if(_pointerProvider != null)
					throw new Exception("PointerProvider is already set");
				_pointerProvider = value;
			}
		}

		private UnitGroup _currentHoveredGroup;




		public void Update()
		{
			var pointerPosition = _pointerProvider.GetPointerPosition();
			var hoveredGroup = Army.Army.KDTree.GetNearestUnits(pointerPosition, 1);
			if(hoveredGroup.Count == 0)
			{
				Dehover();
				return;
			}
			var hoveredUnit = hoveredGroup[0];
			if((hoveredUnit.Position - pointerPosition).sqrMagnitude > _selectionRadius * _selectionRadius)
			{
				Dehover();
				return;
			}
			var hoveredUnitGroup = hoveredUnit.Group;
			if(hoveredUnitGroup != _currentHoveredGroup)
			{
				Dehover();
				_currentHoveredGroup = hoveredUnitGroup;
				OnUnitGroupHovered?.Invoke(_currentHoveredGroup);
			}
			if(Input.GetMouseButtonDown(0))
			{
				OnUnitGroupSelected?.Invoke(_currentHoveredGroup);
			}
		}
		private void Dehover()
		{
			if(_currentHoveredGroup != null)
			{
				OnUnitGroupUnhovered?.Invoke(_currentHoveredGroup);
			}
			_currentHoveredGroup = null;
			return;
		}
	}
}