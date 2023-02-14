using System;
using UnityEngine;

namespace ArmyInput
{
    public class CompositeInputHandler : MonoBehaviour, IInputHandler
    {
        public InputEvents InputEvents { get; } = new InputEvents();
        [SerializeField] private IInputHandler[] _inputHandlers;
        
    }
}
