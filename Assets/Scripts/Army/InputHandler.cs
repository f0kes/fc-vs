using System;
using UnityEngine;

namespace Army
{
	public class InputHandler : MonoBehaviour, IInputHandler
	{
		[SerializeField] private Camera _camera;
		
		public event Action<Vector2> OnMove;
		public event Action<Vector2> MouseWorldPosition;
		public event Action<Vector2> OnClick;

		private void Update()
		{
			var direction = Direction();

			ProcessMouse();
			OnMove?.Invoke(direction);
		}
		private void ProcessMouse()
		{
			//use raycast to get mouse position in world
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if(!Physics.Raycast(ray, out var hit)) return;
			var mouseWorldPosition2D = new Vector2(hit.point.x, hit.point.z);
			MouseWorldPosition?.Invoke(mouseWorldPosition2D);
			if(Input.GetMouseButton(0))
			{
				OnClick?.Invoke(mouseWorldPosition2D);
			}
		}
		private Vector2 Direction()
		{
			//wasd, collect first
			Vector2 direction = Vector2.zero;
			if(Input.GetKey(KeyCode.W))
			{
				direction += Vector2.up;
			}

			if(Input.GetKey(KeyCode.S))
			{
				direction += Vector2.down;
			}

			if(Input.GetKey(KeyCode.A))
			{
				direction += Vector2.left;
			}

			if(Input.GetKey(KeyCode.D))
			{
				direction += Vector2.right;
			}

			//normalize
			if(direction != Vector2.zero)
			{
				direction.Normalize();
			}
			//rotate with camera, camera is top down looking at the ground
			var cameraDirection = _camera.transform.TransformDirection(new Vector3(direction.x, 0, direction.y));
			direction = new Vector2(cameraDirection.x, cameraDirection.z);
			return direction;
		}
	}
}