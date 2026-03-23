using UnityEngine;

namespace States
{
    public abstract class StateMachine : MonoBehaviour
    {
        public bool lockstate = false;
        protected State currentState;
        public State previousState;
    }
}