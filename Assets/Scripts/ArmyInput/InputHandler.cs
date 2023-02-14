using System;
using UnityEngine;

namespace ArmyInput
{
	public class InputHandler : MonoBehaviour, IInputHandler
	{
		[SerializeField] private Camera _camera;

		public InputEvents InputEvents{get;} = new InputEvents();

		private void Update()
		{
			var direction = Direction();

			ProcessMouse();
			InputEvents.OnMove?.Invoke(direction);
		}
		private void ProcessMouse()
		{
			//use raycast to get mouse position in world
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if(!Physics.Raycast(ray, out var hit)) return;
			var mouseWorldPosition2D = new Vector2(hit.point.x, hit.point.z);
			InputEvents.MouseWorldPosition?.Invoke(mouseWorldPosition2D);
			if(Input.GetMouseButton(0))
			{
				InputEvents.OnClick?.Invoke(mouseWorldPosition2D);
			}
			if(Input.GetMouseButton(1))
			{
				InputEvents.OnRightClick?.Invoke(mouseWorldPosition2D);
			}
			if(Input.GetMouseButton(2))
			{
				InputEvents.OnMiddleClick?.Invoke(mouseWorldPosition2D);
			}
			InputEvents.OnScroll?.Invoke(Input.mouseScrollDelta.y);
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
			direction = new Vector2(cameraDirection.x, cameraDirection.z).normalized;
			return direction;
		}
	}
}