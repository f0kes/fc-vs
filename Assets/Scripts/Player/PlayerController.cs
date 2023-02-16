using System;
using ArmyInput;
using MyCamera;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private InputHandler _inputHandler;
		[SerializeField] private AutoInputHandler _botInputHandler;
		[SerializeField] private CameraMover _cameraMover;
		private void Awake()
		{
			
		}
	}
}