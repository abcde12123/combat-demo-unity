using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public PlayerLockOn lockOn;
    public PlayerStamina stamina;
    CharacterController controller;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;
    public float jumpSpeed = 6f;
    public float gravity = 20f;

    float turnSmoothVelocity;
    Vector3 moveDirection;
    Vector3 dodgeDirection;
    float dodgeTimer;
    float currentMovingSpeed;
    float verticalVelocity = -2f;

    public bool IsDodging { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSprinting { get; private set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!cameraTransform) cameraTransform = Camera.main ? Camera.main.transform : null;
        if (!lockOn) lockOn = GetComponent<PlayerLockOn>();
        if (!stamina) stamina = GetComponent<PlayerStamina>();
    }

    public void Tick()
    {
        HandleDodge();
        HandleJumpAndGravity();
        HandleMove();
    }

    void HandleMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        float targetSpeed = 0f;
        if (inputDir.magnitude >= 0.1f)
        {
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
            targetSpeed = IsSprinting ? runSpeed : walkSpeed;
        }
        else
        {
            IsSprinting = false;
        }
        bool allowSprint = stamina.TickSprint(IsSprinting, Time.deltaTime);
        if (!allowSprint) IsSprinting = false;

        currentMovingSpeed = Mathf.Lerp(currentMovingSpeed, targetSpeed, Time.deltaTime * 10f);

        if (inputDir.magnitude >= 0.1f)
        {
            if (!(lockOn && lockOn.IsLocked))
            {
                float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + (cameraTransform ? cameraTransform.eulerAngles.y : transform.eulerAngles.y);
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            else
            {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;
                forward.y = 0; right.y = 0;
                moveDirection = (forward * v + right * h).normalized;
            }
        }

        Vector3 horizontalMove = moveDirection * currentMovingSpeed;
        if (IsDodging) horizontalMove = dodgeDirection * dodgeSpeed;
        Vector3 finalMove = horizontalMove + new Vector3(0f, verticalVelocity, 0f);
        controller.Move(finalMove * Time.deltaTime);
    }

    void HandleDodge()
    {
        if (IsDodging)
        {
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0f) IsDodging = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (stamina.Consume(stamina.dodgeCost))
            {
                IsDodging = true;
                dodgeTimer = dodgeDuration;
                Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
                if (inputDir.sqrMagnitude > 0.01f)
                {
                    float yaw = (lockOn && lockOn.IsLocked) ? transform.eulerAngles.y : (cameraTransform ? cameraTransform.eulerAngles.y : transform.eulerAngles.y);
                    Vector3 world = Quaternion.Euler(0f, yaw, 0f) * inputDir.normalized;
                    dodgeDirection = world;
                }
                else
                {
                    dodgeDirection = transform.forward;
                }
            }
        }
    }

    void HandleJumpAndGravity()
    {
        if (controller.isGrounded)
        {
            IsJumping = false;
            if (verticalVelocity < 0f) verticalVelocity = -2f;
            if (Input.GetKeyDown(KeyCode.Space) && stamina.Consume(stamina.attackLightCost))
            {
                verticalVelocity = jumpSpeed;
                IsJumping = true;
            }
        }
        verticalVelocity -= gravity * Time.deltaTime;
    }
}
