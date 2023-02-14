using System;
using UnityEngine;

namespace ArmyInput
{
	public interface IInputHandler
	{
		public InputEvents InputEvents { get; }
	}
}