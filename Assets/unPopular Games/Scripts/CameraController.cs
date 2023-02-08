using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	/* To turn off the visibility 
	 * of the float inputs in the editor 
	 * just delete [SerializeField] 
	 * from the beginning of each float line */

	public Transform cameraTransform;

	[SerializeField] private float _camMovementSpeed = 1f;
	[SerializeField] private float _camSmoothness = 10f;

	[SerializeField] private float _maxCamZoom = 30f;
	[SerializeField] private float _minCamZoom = 100f;

	[SerializeField] private float _minZCamMovement = 100f;
	[SerializeField] private float _maxZCamMovement = 900f;
	[SerializeField] private float _minXCamMovement = 100f;
	[SerializeField] private float _maxXCamMovement = 900f;

	[SerializeField] private float _zoomSpeed = 2f;

	[SerializeField] private bool cursorVisible = true;
	[SerializeField] private bool _edgePan = true;
	[SerializeField] private bool _enableZoom = true;

	private Vector3 zoomAmount;

	public Vector3 newPosition;
	public Quaternion newRotation;
	public Vector3 newZoom;

	//MouseMovement
	public Vector3 rotateStartPosition;
	public Vector3 rotateCurrentPosition;

	Vector2 pos1;
	Vector2 pos2;

	// Start is called before the first frame update
	void Start()
	{
		newPosition = transform.position;
		newRotation = transform.rotation;
		newZoom = cameraTransform.localPosition;
		zoomAmount = cameraTransform.forward;
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovementInput();
		HandleMouseInput();
	}

	void HandleMouseInput()
	{
		if (_enableZoom) Zoom();
		

		if (Input.GetMouseButtonDown(1))
		{
			rotateStartPosition = Input.mousePosition;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}

		if (Input.GetMouseButton(1))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = cursorVisible;

			newRotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X"));
		}

		if (Input.GetMouseButton(2))
		{
			//Camera movement on mouse button hold
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = cursorVisible;

			newPosition += transform.right * -Input.GetAxis("Mouse X") * _camMovementSpeed;
			newPosition += transform.forward * -Input.GetAxis("Mouse Y") * _camMovementSpeed;
		}
	}

	private void Zoom()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			newZoom += Input.mouseScrollDelta.y * zoomAmount * _zoomSpeed;

			if (newZoom.y <= _maxCamZoom) //Max zoom limit
			{
				newZoom = new Vector3(0, _maxCamZoom, -_maxCamZoom);
			}
			else if (newZoom.y >= _minCamZoom) //Min zoom limit
			{
				newZoom = new Vector3(0, _minCamZoom, -_minCamZoom);
			}
		}
	}

	void HandleMovementInput()
	{
		if (_edgePan) RecordEdgePan();

		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _camSmoothness);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _camSmoothness);
		cameraTransform.localPosition =
			Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * _camSmoothness);
	}

	private void RecordEdgePan()
	{
		//Setting Borders
		if (newPosition.x < _minXCamMovement)
		{
			newPosition = new Vector3(_minXCamMovement, transform.position.y, transform.position.z);
		}
		else if (newPosition.x > _maxXCamMovement)
		{
			newPosition = new Vector3(_maxXCamMovement, transform.position.y, transform.position.z);
		}

		if (newPosition.z < _minZCamMovement)
		{
			newPosition = new Vector3(transform.position.x, transform.position.y, _minZCamMovement);
		}
		else if (newPosition.z > _maxZCamMovement)
		{
			newPosition = new Vector3(transform.position.x, transform.position.y, _maxZCamMovement);
		}
	}
}