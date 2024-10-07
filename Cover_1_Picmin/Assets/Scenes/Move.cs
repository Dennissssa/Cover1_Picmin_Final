using UnityEngine;
using UnityEngine.AI;

public class TestNavAgent : MonoBehaviour
{
    public NavMeshAgent agent; // 导航代理
    public GameObject cylinderPrefab; // 圆柱体预制体
    private GameObject currentCylinder; // 当前生成的圆柱体

    void Update()
    {
        // 检查鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标位置并转换为世界坐标
            Vector2 mousePosition = Input.mousePosition;
            Ray worldRay = Camera.main.ScreenPointToRay(mousePosition);

            // 射线检测
            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                // 如果已有圆柱体，删除它
                if (currentCylinder != null)
                {
                    Destroy(currentCylinder);
                }

                // 生成新的圆柱体并设置位置
                currentCylinder = Instantiate(cylinderPrefab, hitInfo.point + Vector3.up * 0.5f, Quaternion.identity);

                // 设置导航代理的目标位置
                agent.SetDestination(hitInfo.point);

                // 开始协程等待到达
                StartCoroutine(WaitForArrival());
            }
        }
    }

    private System.Collections.IEnumerator WaitForArrival()
    {
        // 等待直到代理到达目标位置
        while (true)
        {
            // 检查角色是否接近目标位置
            if (Vector3.Distance(transform.position, agent.destination) < 0.1f && !agent.pathPending)
            {
                // 删除当前圆柱体
                if (currentCylinder != null)
                {
                    Destroy(currentCylinder);
                    currentCylinder = null; // 清空引用
                }
                yield break; // 退出协程
            }
            yield return null; // 等待下一帧
        }
    }
}