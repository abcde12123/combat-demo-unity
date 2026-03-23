using UnityEngine;
using Movement;

public class PlayerAnimatorSync : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;

    void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        //if (!lockOn) lockOn = GetComponent<PlayerLockOn>();
        if (!movement) movement = GetComponent<PlayerMovement>();
        //if (!combat) combat = GetComponent<PlayerCombat>();
    }

    public void Tick()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = new Vector2(horizontal, vertical).magnitude;

        //animator.SetBool("IsLocked", lockOn && lockOn.IsLocked);
        //animator.SetBool("IsDodging", movement && movement.IsDodging);
        //animator.SetBool("IsAttacking", combat && combat.IsAttacking);
        //animator.SetBool("IsJumping", movement && movement.IsJumping);
        //animator.SetBool("IsSprinting", movement && movement.IsSprinting);

        /*if (!(lockOn && lockOn.IsLocked))
        {
            animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("InputX", horizontal, 0.1f, Time.deltaTime);
            animator.SetFloat("InputY", vertical, 0.1f, Time.deltaTime);
        }*/
    }
}
