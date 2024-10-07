using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{
    public NavMeshAgent[] agents; // ������������
    public GameObject cylinderPrefab; // Բ����Ԥ����
    public GameObject[] characters; // ��ɫ����
    private GameObject currentCylinder; // ��ǰ���ɵ�Բ����
    private GameObject selectedCharacter; // ��ǰѡ�еĽ�ɫ

    void Update()
    {
        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            // ��ȡ���λ�ò�ת��Ϊ��������
            Vector2 mousePosition = Input.mousePosition;
            Ray worldRay = Camera.main.ScreenPointToRay(mousePosition);

            // ���߼��
            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                // ����Ƿ����˽�ɫ
                foreach (var character in characters)
                {
                    if (hitInfo.collider.gameObject == character)
                    {
                        HandleCharacterSelection(character);
                        return;
                    }
                }

                // ����Ƿ����˱���
                if (hitInfo.collider.TryGetComponent<Treasure>(out Treasure treasure))
                {
                    if (selectedCharacter != null)
                    {
                        MoveSelectedCharacter(treasure.transform.position); // �ƶ�������λ��
                        treasure.AttachCharacter(selectedCharacter); // ���ӽ�ɫ������
                    }
                    else
                    {
                        treasure.DismissAllCharacters();
                    }
                    return;
                }

                // �������˵��棬�ƶ�ѡ�еĽ�ɫ
                if (selectedCharacter != null)
                {
                    MoveSelectedCharacter(hitInfo.point);
                }
            }
        }

        // �������Ҽ������ȡ��ѡ��
        if (Input.GetMouseButtonDown(1) && selectedCharacter != null)
        {
            DeselectCharacter();
        }
    }

    private void HandleCharacterSelection(GameObject character)
    {
        // ����Ѿ�ѡ����������ɫ��ȡ��ѡ��
        if (selectedCharacter != null)
        {
            DeselectCharacter();
        }

        // ѡ���½�ɫ����ʾ�⻷
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
            selectedCharacter = null; // ���ѡ��
        }
    }

    private void ShowChildObjects(GameObject character, bool isVisible)
    {
        foreach (Transform child in character.transform)
        {
            child.gameObject.SetActive(isVisible); // ��ʾ������������
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
            // ���ô����Ŀ��λ��
            selectedAgent.SetDestination(destination);

            // �������Բ���壬ɾ����
            if (currentCylinder != null)
            {
                Destroy(currentCylinder);
            }

            // �����µ�Բ���岢����λ��
            currentCylinder = Instantiate(cylinderPrefab, destination + Vector3.up * 0.5f, Quaternion.identity);

            // ��ʼЭ�̵ȴ�����
            StartCoroutine(WaitForArrival(selectedAgent));
        }
    }

    private System.Collections.IEnumerator WaitForArrival(NavMeshAgent agent)
    {
        while (true)
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) < 0.1f && !agent.pathPending)
            {
                // ����ɫ�Ƿ񿿽�����
                Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, 1f); // 1f �Ǽ��뾶
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent<Treasure>(out Treasure treasure))
                    {
                        treasure.AttachCharacter(selectedCharacter); // ���ӽ�ɫ������
                    }
                }

                if (currentCylinder != null)
                {
                    Destroy(currentCylinder);
                    currentCylinder = null; // �������
                }
                yield break; // �˳�Э��
            }
            yield return null;
        }
    }
}