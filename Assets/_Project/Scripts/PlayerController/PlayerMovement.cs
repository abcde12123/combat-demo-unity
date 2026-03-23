using System;
using Inputs;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Transform _playerTransform;

        [Header("component")] [SerializeField] private CharacterController _characterController;

        [Header("FreeLockState")] public float FreeLockMaxSpeed = 10f;
        public float FreeLockSpeed = 10f;
        public float FreeLockRotationLerpTime = 3f;

        public Vector3 velocity => _characterController.velocity;

        public CharacterController CharacterController
        {
            get => _characterController;
            set => _characterController = value;
        }

        private Transform _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main.transform;
        }

        public Vector3 CamRelativeMotionVector(Vector2 input2DMovementVector)
        {
            Vector3 forwardVector = _mainCamera.forward * input2DMovementVector.y;
            Vector3 rightVector = _mainCamera.right * input2DMovementVector.x;
            Vector3 relativeVector = forwardVector + rightVector;
            relativeVector.y = 0f;

            return relativeVector;
        }

        public Vector3 TargetRelativeMotionVector(Vector3 targetPos)
        {
            Vector3 relativeVector = targetPos - transform.position;
            relativeVector.y = 0f;
            return relativeVector;
        }

        public void LookRotation(Vector3 movementVector, float deltaTime)
        {
            if (movementVector != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation, Quaternion.LookRotation(movementVector), deltaTime * FreeLockRotationLerpTime);
            }
        }

        public void Move(Vector3 movementVector, float speed, float deltaTime)
        {
            _characterController.Move(movementVector * (speed * deltaTime));
        }

        public void RotateHumanModel(float angle)
        {
            _playerTransform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }
}