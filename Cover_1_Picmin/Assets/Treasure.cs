using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Treasure : MonoBehaviour
{
    public List<GameObject> attachedCharacters = new List<GameObject>(); // ���ӵĽ�ɫ
    public float moveSpeed = 2f; // �����ƶ��ٶ�
    public Transform homeBase; // �һ���λ��
    public GameObject indicator; // ָʾ������
    private bool isMoving = false; // �����Ƿ������ƶ�
    public string SceneName;

    void Start()
    {
        if (indicator != null)
        {
            indicator.SetActive(false); // ��ʼ����ָʾ��
        }
    }

    void Update()
    {
        // �ƶ��߼�
        if (isMoving)
        {
            MoveTreasure();
            UpdateAttachedCharacters();
        }
    }

    public void AttachCharacter(GameObject character)
    {
        if (!attachedCharacters.Contains(character))
        {
            attachedCharacters.Add(character);
            character.transform.SetParent(transform); // �趨��ɫ�ĸ�����Ϊ����
            character.GetComponent<CharacterState>().SetState(CharacterState.State.Carrying);

            // ��ʾָʾ��
            if (indicator != null)
            {
                indicator.SetActive(true);
            }
        }

        // �����������ɫ����ʼ�ƶ�����
        if (attachedCharacters.Count >= 2)
        {
            StartMovingTreasure();
        }
    }

    private void StartMovingTreasure()
    {
        isMoving = true;
    }

    private void MoveTreasure()
    {
        // �ƶ��߼�
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, homeBase.position, moveSpeed * Time.deltaTime);
        transform.position = targetPosition;

        // ����Ƿ񵽴�һ���
        if (Vector3.Distance(transform.position, homeBase.position) < 0.1f)
        {
            CollectTreasure();
        }
    }

    private void UpdateAttachedCharacters()
    {
        // ���¸��ӽ�ɫ��λ�ã�ȷ����ɫʼ�ո��汦��
        foreach (var character in attachedCharacters)
        {
            character.transform.position = transform.position; // ��ɫ���汦��
        }
    }

    private void CollectTreasure()
    {
        // �ռ����ص��߼�
        if (indicator != null)
        {
            Destroy(indicator); // ɾ��ָʾ��
        }
        Destroy(gameObject); // ɾ������
        DismissAllCharacters();
        SwitchScene(SceneName);
    }

    public void DismissAllCharacters()
    {
        foreach (var character in attachedCharacters)
        {
            character.transform.SetParent(null); // �����ɫ�ĸ�����
            character.GetComponent<CharacterState>().SetState(CharacterState.State.Idle);
        }
        attachedCharacters.Clear();
    }
    private void SwitchScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName); // �л�����
        }
        else
        {
            Debug.LogError("���� " + sceneName + " �����ڻ�δ���ء�");
        }
    }
}