using System;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyCamera
{
	public class CameraMover : MonoBehaviour, IPointerProvider
	{
		[SerializeField] private Transform _cameraTarget;
		[SerializeField] private Transform _pivotTransform;
		[SerializeField] private Camera _camera;

		[SerializeField] private float _movementAcceleration = 1f;
		[SerializeField] private float _movementDamper = 0.1f;

		[SerializeField] private float _zoomPercentage = 0.1f;
		[SerializeField] private float _zoomMin = 1f;
		[SerializeField] private float _zoomMax = 10f;

		[SerializeField] private float _minRotation = 15f;
		[SerializeField] private float _maxRotation = 90f;

		[SerializeField] private Transform _followTarget;

		private Vector2 _pivotLastFrame;
		private Vector2 _pivotPosition;
		private Vector2 _pivotDelta;
		private Vector3 _dragOrigin;

		private Vector3 _desiredPosition;
		private Vector2 _targetVelocity;

		private Vector3 _rotationPoint;
		private float _rotationX;

		private Vector3 _zoomDesiredPosition;
		
		private Vector3 _followTargetLastFrame;

		private bool _isDragging;
		private bool _isRotating;
		private void Start()
		{
			var position = _pivotTransform.position;
			_pivotPosition = new Vector2(position.x, position.z);
			_pivotLastFrame = _pivotPosition;
			_cameraTarget.position = new Vector3(_pivotPosition.x, _zoomMin, _pivotPosition.y);
			_rotationX = _cameraTarget.transform.eulerAngles.x;
		}
		private void LateUpdate()
		{
			UpdatePivotPosition();
			Move();
			Zoom();
			Rotate();
			AdjustToTarget();
		}
		private void UpdatePivotPosition()
		{
			_pivotLastFrame = _pivotPosition;
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if(!Physics.Raycast(ray, out var hit)) return;
			_pivotPosition = new Vector2(hit.point.x, hit.point.z);
			_pivotTransform.position = new Vector3(_pivotPosition.x, 0, _pivotPosition.y);
		}
		private void Move()
		{
			_pivotDelta = _pivotPosition - _pivotLastFrame;
			if(Input.GetMouseButtonDown(0))
			{
				_isDragging = true;
				_dragOrigin = _pivotTransform.position;
			}
			if(Input.GetMouseButtonUp(0))
			{
				_isDragging = false;
			}
			UpdateTargetPosition(-_pivotDelta);
		}

		private void UpdateTargetPosition(Vector2 delta)
		{
			var targetPos = _cameraTarget.position;
			if(_isDragging)
			{
				var targetToPivot = _pivotTransform.position - targetPos;
				var cameraTargetPos = _cameraTarget.position;
				_desiredPosition = _dragOrigin - targetToPivot;

				var newPos = _cameraTarget.position = Vector3.Lerp(targetPos, _desiredPosition, Time.deltaTime * _movementAcceleration);
				var deltaPos = newPos - cameraTargetPos;
				_targetVelocity = new Vector2(deltaPos.x, deltaPos.z) / Time.deltaTime;
			}
			else
			{
				_cameraTarget.position += new Vector3(_targetVelocity.x, 0, _targetVelocity.y) * Time.deltaTime;
				_targetVelocity *= (1 - _movementDamper * Time.deltaTime);
			}
		}


		private void Zoom()
		{
			var zoom = -Input.mouseScrollDelta.y * _zoomPercentage;

			var targetPos = _cameraTarget.position;
			var zoomDirection = (_camera.transform.position - _pivotTransform.position);
			_zoomDesiredPosition = targetPos + zoomDirection * zoom;
			if(!(_zoomDesiredPosition.y >= _zoomMin && _zoomDesiredPosition.y <= _zoomMax))
			{
				_zoomDesiredPosition.y = Mathf.Clamp(targetPos.y, _zoomMin, _zoomMax);
				_zoomDesiredPosition = new Vector3(targetPos.x, _zoomDesiredPosition.y, targetPos.z);
			}
			_cameraTarget.position = _zoomDesiredPosition;
		}
		private void Rotate()
		{
			if(Input.GetMouseButtonDown(1))
			{
				_isRotating = true;
				_rotationPoint = _pivotTransform.position;
			}
			if(Input.GetMouseButtonUp(1))
			{
				_isRotating = false;
			}

			if(_isRotating)
			{
				var x = Input.GetAxis("Mouse X");
				var y = -Input.GetAxis("Mouse Y");

				if(_rotationX + y < _minRotation)
				{
					y = _minRotation - _rotationX;
				}
				else if(_rotationX + y > _maxRotation)
				{
					y = _maxRotation - _rotationX;
				}

				_cameraTarget.RotateAround(_rotationPoint, Vector3.up, x);
				_cameraTarget.RotateAround(_rotationPoint, _cameraTarget.transform.right, y);

				_rotationX += y;
			}
		}

		private void AdjustToTarget()
		{
			var followPos = _followTarget.position;
			var targetPos = _cameraTarget.position;
			var followDelta = followPos - _followTargetLastFrame;
			followPos.y = 0;
			targetPos += followDelta;
			_cameraTarget.position = targetPos;
			_followTargetLastFrame = followPos;
		}

		public Vector2 GetPointerPosition()
		{
			return _pivotPosition;
		}
	}

}