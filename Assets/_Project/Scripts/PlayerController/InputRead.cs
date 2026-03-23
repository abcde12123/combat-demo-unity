using UnityEngine;
using UnityEngine.InputSystem;

//using combat;
namespace Inputs
{
    public class InputRead : MonoBehaviour, PlayerControls.IPlayerActions
    {
        private PlayerControls _Controls;
        public bool StopInput;

        public Vector2 MovementOn2DAxis
        {
            get; 
            private set;
        }

        public Vector2 LookOn2DAxis
        {
            get;
            private set;
        }
        
        public event System.Action JumpEvent;
        public event System.Action LightAttackEvent;
        public event System.Action DodgeEvent;
        public event System.Action LockEvent;

        void Awake()
        {
            _Controls = new PlayerControls();
            _Controls.Player.SetCallbacks(this);
            _Controls.Player.Enable();
        }

        void OnEnable()
        {
            _Controls.Player.Enable();
        }

        void OnDisable()
        {
            _Controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (StopInput)
            {
                return;
            }
            MovementOn2DAxis= context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookOn2DAxis= context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.performed)
            {
                return;
            }
            JumpEvent?.Invoke();
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            LightAttackEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            DodgeEvent?.Invoke();
        }

        public void OnLock(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            LockEvent?.Invoke();
        }
    }
}

