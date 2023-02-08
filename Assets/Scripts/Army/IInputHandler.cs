using System;
using UnityEngine;

namespace Army
{
	public interface IInputHandler
	{
		event Action<Vector2> OnMove;
		event Action<Vector2> MouseWorldPosition;
		event Action<Vector2> OnClick;
	}
}