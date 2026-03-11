using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float max = 100f;
    public float value = 100f;
    public float regen = 20f;
    public float dodgeCost = 20f;
    public float attackLightCost = 15f;
    public float attackHeavyCost = 30f;
    public float sprintCostPerSecond = 10f;

    public bool Consume(float amount)
    {
        if (value < amount) return false;
        value -= amount;
        return true;
    }

    public bool TickSprint(bool sprinting, float deltaTime)
    {
        if (sprinting)
        {
            value = Mathf.Max(0f, value - sprintCostPerSecond * deltaTime);
            return value > 0f;
        }
        value = Mathf.Min(max, value + regen * deltaTime);
        return true;
    }
}
