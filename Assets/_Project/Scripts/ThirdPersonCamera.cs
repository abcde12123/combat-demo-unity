using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("目标设置")]
    public Transform target;        // 拖入 Player
    public Vector3 offset = new Vector3(0, 1.5f, 0); // 角色头顶偏移

    [Header("距离设置")]
    public float distance = 5.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;

    [Header("速度设置")]
    public float xSpeed = 200.0f;
    public float ySpeed = 120.0f;
    public float smoothTime = 0.1f;

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 初始化角度
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate() // 严谨：相机必须在 LateUpdate
    {
        if (!target) return;

        // 1. 获取输入
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        y = Mathf.Clamp(y, -20, 80);

        // 2. 平滑旋转
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(y, x, 0), ref rotationVelocity, smoothTime);
        transform.eulerAngles = currentRotation;

        // 3. 滚轮缩放
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);

        // 4. 计算位置
        transform.position = (target.position + offset) - transform.forward * distance;
    }
}