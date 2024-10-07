using UnityEngine;
using UnityEngine.AI;

public class NPCContro : MonoBehaviour
{
    public float followDistance = 5f; // 跟随距离
    public float speed = 3.5f; // NPC 跟随速度
    public string targetTag = "Target"; // 特定物体的标签

    private Transform player; // 主角的 Transform
    private NavMeshAgent agent; // 导航代理
    private bool isFollowing = true; // 跟随状态

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 查找主角
        agent.speed = speed; // 设置导航代理的速度
    }

    void Update()
    {
        if (isFollowing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 检查 NPC 是否在指定的跟随距离内
            if (distanceToPlayer < followDistance)
            {
                // 跟随主角
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath(); // 超出跟随距离，停止移动
            }

            // 检查是否触碰到特定标签的物体
            if (IsTouchingTarget())
            {
                agent.ResetPath(); // 停止移动
                isFollowing = false; // 设置为不再跟随
                Debug.Log("NPC reached the target."); // 调试信息
            }
        }
    }

    private bool IsTouchingTarget()
    {
        // 使用球体检测是否触碰到特定标签的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                return true; // 如果碰撞体的标签匹配，返回 true
            }
        }
        return false; // 未找到匹配的标签
    }

    private void OnDrawGizmos()
    {
        // 在场景视图中显示检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // 检测半径
    }
}