using System;
using UnityEngine;

namespace ArmyInput
{
    public class CompositeInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2> OnMove;
        public event Action<Vector2> MouseWorldPosition;
        public event Action<Vector2> OnClick;
        [SerializeField] private IInputHandler[] _inputHandlers;
        
    }
}
