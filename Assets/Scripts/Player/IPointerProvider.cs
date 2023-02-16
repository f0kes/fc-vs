using UnityEngine;

namespace Player
{
	public interface IPointerProvider
	{
		public Vector2 GetPointerPosition();
	}
}