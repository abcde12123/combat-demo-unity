using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerStamina stamina;
    public bool IsAttacking { get; private set; }
    float attackTimer;
    int bufferedAttack;

    void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stamina) stamina = GetComponent<PlayerStamina>();
    }

    public void Tick()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                IsAttacking = false;
                if (bufferedAttack != 0)
                {
                    Execute(bufferedAttack);
                    bufferedAttack = 0;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsAttacking) bufferedAttack = 1;
            else Execute(1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (IsAttacking) bufferedAttack = 2;
            else Execute(2);
        }
    }

    void Execute(int type)
    {
        if (type == 1)
        {
            if (!stamina.Consume(stamina.attackLightCost)) return;
            animator.SetTrigger("AttackLight");
        }
        else
        {
            if (!stamina.Consume(stamina.attackHeavyCost)) return;
            animator.SetTrigger("AttackHeavy");
        }
        IsAttacking = true;
        attackTimer = 0.4f;
    }
}
