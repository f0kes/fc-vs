using System;
using UnityEngine;

namespace ArmyInput
{
	public class InputEvents
	{
		public  Action<Vector2> OnMove;
		public  Action<Vector2> MouseWorldPosition;
		public  Action<Vector2> OnClick;
		public  Action<Vector2> OnRightClick;
		public  Action<Vector2> OnMiddleClick; 
		public  Action<float> OnScroll;
		
	}
}