using System;
using Army;
using UnityEngine;

namespace ArmyInput
{
    public class DummyInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2> OnMove;
        public event Action<Vector2> MouseWorldPosition;
        public event Action<Vector2> OnClick;
    }
}
