using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    public Transform lockTarget;
    public LayerMask lockableMask = ~0;
    public float lockOnRadius = 12f;
    public bool IsLocked { get; private set; }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!IsLocked)
            {
                AcquireLockTarget();
                IsLocked = lockTarget != null;
            }
            else
            {
                IsLocked = false;
                lockTarget = null;
            }
        }
        if (IsLocked && lockTarget)
        {
            Vector3 lookPos = lockTarget.position - transform.position;
            lookPos.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }

    void AcquireLockTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRadius, lockableMask, QueryTriggerInteraction.Ignore);
        Transform best = null;
        float bestDist = float.MaxValue;
        for (int i = 0; i < hits.Length; i++)
        {
            Transform t = hits[i].transform;
            if (t == transform) continue;
            float d = Vector3.SqrMagnitude(t.position - transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = t;
            }
        }
        lockTarget = best;
    }
}
