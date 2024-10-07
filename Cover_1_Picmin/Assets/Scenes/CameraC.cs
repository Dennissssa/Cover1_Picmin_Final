using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // 控制相机移动速度

    void Update()
    {
        // 获取输入
        float moveHorizontal = Input.GetAxis("Horizontal"); // A和D键
        float moveVertical = Input.GetAxis("Vertical"); // W和S键

        // 计算移动方向，只在 X 和 Z 轴上
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * moveSpeed * Time.deltaTime;

        // 移动相机
        transform.Translate(movement, Space.World);
    }
}