using UnityEngine;
using Unity.AI.Navigation;

public class NpcCollisionController : MonoBehaviour
{
    public GameObject objectToDelete; // Ҫɾ���Ĺ�������
    public string npcTag = "NPC"; // NPC ��ǩ
    public NavMeshSurface navMeshSurface;

    private void OnTriggerEnter(Collider other)
    {
        // ������Ķ����Ƿ���� npcTag ��ǩ
        if (other.CompareTag(npcTag))
        {
            // ���� NPC��������Ⱦ���������� NavMesh Agent��
            HideNpc(other.gameObject);

            // ɾ����װ�˸ýű��� GameObject
            if (objectToDelete != null)
            {
                Destroy(objectToDelete); // ɾ��ָ��������
                Debug.Log("ɾ������: " + objectToDelete.name);
                navMeshSurface.BuildNavMesh();
            }
            else
            {
                Debug.LogWarning("δ����Ҫɾ�������塣");
            }

            // ɾ����װ�˸ýű��ı���
            Destroy(gameObject); // ɾ����ǰ GameObject
            Debug.Log("ɾ������������: " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ��ѡ: �� NPC �뿪ʱִ��ĳЩ����
        if (other.CompareTag(npcTag))
        {
            Debug.Log("NPC �뿪������: " + other.name);
        }
    }

    private void HideNpc(GameObject npc)
    {
        // ���� NPC ����Ⱦ���������� NavMesh Agent
        Renderer[] renderers = npc.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false; // ������Ⱦ��
        }

        // ȷ�� NavMesh Agent ��Ȼ����
        UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true; // ȷ�� NavMesh Agent ��Ȼ����
        }

        Debug.Log("���� NPC: " + npc.name);
    }
}