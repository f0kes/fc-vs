using Army.Units;
using ArmyInput;
using MyCamera;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private InputHandler _inputHandler;
		[SerializeField] private CameraMover _cameraMover;
		[SerializeField] private UnitGroupSelector _unitGroupSelector;

		private UnitGroup _selectedGroup;
		private void Awake()
		{
			_unitGroupSelector.PointerProvider = _cameraMover;
			_unitGroupSelector.OnUnitGroupSelected += OnUnitGroupSelected;
			_unitGroupSelector.OnUnitGroupHovered += OnUnitGroupHovered;
			_unitGroupSelector.OnUnitGroupUnhovered += OnUnitGroupUnhovered;
		}

		private void OnUnitGroupUnhovered(UnitGroup group)
		{
			if(group == _selectedGroup) return;
			group.Units.ForEach(unit => unit.Animator.ColorAdd = new Color(1, 1, 1, 0));
		}

		private void OnUnitGroupHovered(UnitGroup group)
		{
			if(group == _selectedGroup) return;
			group.Units.ForEach(unit => unit.Animator.ColorAdd = new Color(1, 1, 1, 0.4f));
		}

		private void OnUnitGroupSelected(UnitGroup group)
		{
			if(_selectedGroup != null)
			{
				_selectedGroup.Army.ResetInputHandler();
				_selectedGroup.Units.ForEach(unit => unit.Animator.ColorAdd = new Color(1, 1, 1, 0));
			}
			
			group.Army.SetInputHandler(_inputHandler);

			_selectedGroup = group;
			group.Units.ForEach(unit => unit.Animator.ColorAdd = new Color(1, 1, 1, 1));
		}
	}
}