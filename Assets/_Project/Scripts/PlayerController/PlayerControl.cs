using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform cameraTransform;
    public Transform lockTarget;
    public LayerMask lockableMask = ~0;
    public bool isLocked;

    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float turnSmoothTime = 0.1f;

    PlayerLockOn lockOn;
    PlayerStamina staminaComp;
    PlayerCombat combat;
    PlayerMovement movement;
    PlayerAnimatorSync animatorSync;

    CharacterController controller;

    public float staminaMax = 100f;
    public float stamina = 100f;
    public float staminaRegen = 20f;
    public float dodgeCost = 20f;
    public float attackLightCost = 15f;
    public float attackHeavyCost = 30f;
    public float sprintCostPerSecond = 10f;
    public float lockOnRadius = 12f;

    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;
    public float jumpSpeed = 6f;
    public float gravity = 20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        lockOn = GetComponent<PlayerLockOn>();
        staminaComp = GetComponent<PlayerStamina>();
        combat = GetComponent<PlayerCombat>();
        movement = GetComponent<PlayerMovement>();
        animatorSync = GetComponent<PlayerAnimatorSync>();
        if (!lockOn) lockOn = gameObject.AddComponent<PlayerLockOn>();
        if (!staminaComp) staminaComp = gameObject.AddComponent<PlayerStamina>();
        if (!combat) combat = gameObject.AddComponent<PlayerCombat>();
        if (!movement) movement = gameObject.AddComponent<PlayerMovement>();
        if (!animatorSync) animatorSync = gameObject.AddComponent<PlayerAnimatorSync>();

        movement.cameraTransform = cameraTransform;
        movement.lockOn = lockOn;
        movement.stamina = staminaComp;
        movement.walkSpeed = walkSpeed;
        movement.runSpeed = runSpeed;
        movement.turnSmoothTime = turnSmoothTime;
        movement.dodgeSpeed = dodgeSpeed;
        movement.dodgeDuration = dodgeDuration;
        movement.jumpSpeed = jumpSpeed;
        movement.gravity = gravity;

        staminaComp.max = staminaMax;
        staminaComp.value = stamina;
        staminaComp.regen = staminaRegen;
        staminaComp.dodgeCost = dodgeCost;
        staminaComp.attackLightCost = attackLightCost;
        staminaComp.attackHeavyCost = attackHeavyCost;
        staminaComp.sprintCostPerSecond = sprintCostPerSecond;

        lockOn.lockableMask = lockableMask;
        lockOn.lockOnRadius = lockOnRadius;
        animatorSync.animator = animator;
        animatorSync.lockOn = lockOn;
        animatorSync.movement = movement;
        animatorSync.combat = combat;
        combat.animator = animator;
        combat.stamina = staminaComp;
    }

    void Update()
    {
        lockOn.Tick();
        combat.Tick();
        movement.Tick();
        animatorSync.Tick();
        lockTarget = lockOn.lockTarget;
        isLocked = lockOn.IsLocked;
        stamina = staminaComp.value;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 90), "玩家状态信息");
        GUI.Label(new Rect(20, 30, 180, 20), "锁定状态: " + ((lockOn && lockOn.IsLocked) ? "已锁定" : "自由视角"));
        GUI.Label(new Rect(20, 50, 180, 20), "当前速度: " + controller.velocity.magnitude.ToString("F2"));
        GUI.Label(new Rect(20, 70, 180, 20), "锁定目标: " + (lockOn && lockOn.lockTarget ? lockOn.lockTarget.name : "无"));
        GUI.Label(new Rect(20, 90, 180, 20), "体力: " + Mathf.RoundToInt(staminaComp.value) + "/" + Mathf.RoundToInt(staminaComp.max));
    }
}
