using UnityEngine;

namespace ArmyInput
{
	public class AutoInputHandler : MonoBehaviour, IInputHandler
	{
		private Army.Army _controlledArmy;
		public InputEvents InputEvents { get; } = new InputEvents();
		public void SetTarget(Army.Army army)
		{
			_controlledArmy = army;
		}
	}
	
}