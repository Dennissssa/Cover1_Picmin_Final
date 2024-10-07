using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{
    public NavMeshAgent[] agents; // 导航代理数组
    public GameObject cylinderPrefab; // 圆柱体预制体
    public GameObject[] characters; // 角色数组
    private GameObject currentCylinder; // 当前生成的圆柱体
    private GameObject selectedCharacter; // 当前选中的角色

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
                // 检查是否点击了角色
                foreach (var character in characters)
                {
                    if (hitInfo.collider.gameObject == character)
                    {
                        HandleCharacterSelection(character);
                        return;
                    }
                }

                // 检查是否点击了宝藏
                if (hitInfo.collider.TryGetComponent<Treasure>(out Treasure treasure))
                {
                    if (selectedCharacter != null)
                    {
                        MoveSelectedCharacter(treasure.transform.position); // 移动到宝藏位置
                        treasure.AttachCharacter(selectedCharacter); // 附加角色到宝藏
                    }
                    else
                    {
                        treasure.DismissAllCharacters();
                    }
                    return;
                }

                // 如果点击了地面，移动选中的角色
                if (selectedCharacter != null)
                {
                    MoveSelectedCharacter(hitInfo.point);
                }
            }
        }

        // 检查鼠标右键点击以取消选择
        if (Input.GetMouseButtonDown(1) && selectedCharacter != null)
        {
            DeselectCharacter();
        }
    }

    private void HandleCharacterSelection(GameObject character)
    {
        // 如果已经选择了其他角色，取消选择
        if (selectedCharacter != null)
        {
            DeselectCharacter();
        }

        // 选择新角色并显示光环
        selectedCharacter = character;
        selectedCharacter.GetComponent<CharacterState>().SetState(CharacterState.State.Active);
        ShowChildObjects(selectedCharacter, true);
    }

    private void DeselectCharacter()
    {
        if (selectedCharacter != null)
        {
            ShowChildObjects(selectedCharacter, false);
            selectedCharacter.GetComponent<CharacterState>().SetState(CharacterState.State.Idle);
            selectedCharacter = null; // 清空选择
        }
    }

    private void ShowChildObjects(GameObject character, bool isVisible)
    {
        foreach (Transform child in character.transform)
        {
            child.gameObject.SetActive(isVisible); // 显示或隐藏子物体
        }
    }

    private void MoveSelectedCharacter(Vector3 destination)
    {
        NavMeshAgent selectedAgent = null;
        foreach (var agent in agents)
        {
            if (agent.gameObject == selectedCharacter)
            {
                selectedAgent = agent;
                break;
            }
        }

        if (selectedAgent != null)
        {
            // 设置代理的目标位置
            selectedAgent.SetDestination(destination);

            // 如果已有圆柱体，删除它
            if (currentCylinder != null)
            {
                Destroy(currentCylinder);
            }

            // 生成新的圆柱体并设置位置
            currentCylinder = Instantiate(cylinderPrefab, destination + Vector3.up * 0.5f, Quaternion.identity);

            // 开始协程等待到达
            StartCoroutine(WaitForArrival(selectedAgent));
        }
    }

    private System.Collections.IEnumerator WaitForArrival(NavMeshAgent agent)
    {
        while (true)
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) < 0.1f && !agent.pathPending)
            {
                // 检查角色是否靠近宝藏
                Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, 1f); // 1f 是检测半径
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent<Treasure>(out Treasure treasure))
                    {
                        treasure.AttachCharacter(selectedCharacter); // 附加角色到宝藏
                    }
                }

                if (currentCylinder != null)
                {
                    Destroy(currentCylinder);
                    currentCylinder = null; // 清空引用
                }
                yield break; // 退出协程
            }
            yield return null;
        }
    }
}