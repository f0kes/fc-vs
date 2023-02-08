using System;
using UnityEngine;

namespace Army
{
    public class DummyInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2> OnMove;
        public event Action<Vector2> MouseWorldPosition;
        public event Action<Vector2> OnClick;
    }
}
