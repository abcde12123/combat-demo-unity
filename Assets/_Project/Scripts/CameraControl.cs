using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("目标设置")]
    public Transform target;        // 拖入 Player
    public Vector3 offset = new Vector3(0, 1.5f, 0); // 角色头顶偏移
    public float lockFocusBlend = 0.6f;

    [Header("距离设置")]
    public float distance = 5.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;
    public float collisionRadius = 0.2f;
    public float collisionBack = 0.05f;

    [Header("速度设置")]
    public float xSpeed = 200.0f;
    public float ySpeed = 120.0f;
    public float smoothTime = 0.1f;

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;
    private PlayerController player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 初始化角度
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        if (target) player = target.GetComponent<PlayerController>();
    }

    void LateUpdate() // 严谨：相机必须在 LateUpdate
    {
        if (!target) return;

        bool locked = player && player.isLocked && player.lockTarget;
        if (!locked)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = Mathf.Clamp(y, -20, 80);
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(y, x, 0), ref rotationVelocity, smoothTime);
            transform.eulerAngles = currentRotation;
        }
        else
        {
            Vector3 focus = target.position + offset;
            Vector3 aim = player.lockTarget.position;
            aim.y = focus.y;
            Quaternion desired = Quaternion.LookRotation((aim - focus).normalized);
            Vector3 desiredEuler = desired.eulerAngles;
            currentRotation = Vector3.SmoothDamp(currentRotation, desiredEuler, ref rotationVelocity, smoothTime);
            transform.eulerAngles = currentRotation;
        }

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);

        Vector3 focusPoint;
        if (!locked)
        {
            focusPoint = target.position + offset;
        }
        else
        {
            Vector3 playerFocus = target.position + offset;
            Vector3 targetFocus = player.lockTarget.position;
            targetFocus.y = playerFocus.y;
            focusPoint = Vector3.Lerp(playerFocus, targetFocus, lockFocusBlend);
        }
        Vector3 desiredPos = focusPoint - transform.forward * distance;
        Vector3 dir = (desiredPos - focusPoint).normalized;
        float dist = Vector3.Distance(focusPoint, desiredPos);
        if (Physics.SphereCast(focusPoint, collisionRadius, dir, out RaycastHit hit, dist))
        {
            float d = Mathf.Clamp(hit.distance - collisionBack, minDistance, maxDistance);
            distance = Mathf.Lerp(distance, d, 0.5f);
            desiredPos = focusPoint - transform.forward * distance;
        }
        transform.position = desiredPos;
    }
}
