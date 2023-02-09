using System;
using UnityEngine;

namespace ArmyInput
{
	public interface IInputHandler
	{
		event Action<Vector2> OnMove;
		event Action<Vector2> MouseWorldPosition;
		event Action<Vector2> OnClick;
	}
}