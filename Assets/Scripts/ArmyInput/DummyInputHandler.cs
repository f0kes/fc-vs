using System;
using Army;
using UnityEngine;

namespace ArmyInput
{
    public class DummyInputHandler : MonoBehaviour, IInputHandler
    {

        public InputEvents InputEvents{get;} = new InputEvents();
    }
}
