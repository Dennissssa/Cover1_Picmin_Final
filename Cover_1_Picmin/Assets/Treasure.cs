using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Treasure : MonoBehaviour
{
    public List<GameObject> attachedCharacters = new List<GameObject>(); // 附加的角色
    public float moveSpeed = 2f; // 宝藏移动速度
    public Transform homeBase; // 家基地位置
    public GameObject indicator; // 指示器对象
    private bool isMoving = false; // 宝藏是否正在移动
    public string SceneName;

    void Start()
    {
        if (indicator != null)
        {
            indicator.SetActive(false); // 初始隐藏指示器
        }
    }

    void Update()
    {
        // 移动逻辑
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
            character.transform.SetParent(transform); // 设定角色的父物体为宝藏
            character.GetComponent<CharacterState>().SetState(CharacterState.State.Carrying);

            // 显示指示器
            if (indicator != null)
            {
                indicator.SetActive(true);
            }
        }

        // 如果有两个角色，开始移动宝藏
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
        // 移动逻辑
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, homeBase.position, moveSpeed * Time.deltaTime);
        transform.position = targetPosition;

        // 检查是否到达家基地
        if (Vector3.Distance(transform.position, homeBase.position) < 0.1f)
        {
            CollectTreasure();
        }
    }

    private void UpdateAttachedCharacters()
    {
        // 更新附加角色的位置，确保角色始终跟随宝藏
        foreach (var character in attachedCharacters)
        {
            character.transform.position = transform.position; // 角色跟随宝藏
        }
    }

    private void CollectTreasure()
    {
        // 收集宝藏的逻辑
        if (indicator != null)
        {
            Destroy(indicator); // 删除指示器
        }
        Destroy(gameObject); // 删除宝藏
        DismissAllCharacters();
        SwitchScene(SceneName);
    }

    public void DismissAllCharacters()
    {
        foreach (var character in attachedCharacters)
        {
            character.transform.SetParent(null); // 解除角色的父物体
            character.GetComponent<CharacterState>().SetState(CharacterState.State.Idle);
        }
        attachedCharacters.Clear();
    }
    private void SwitchScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName); // 切换场景
        }
        else
        {
            Debug.LogError("场景 " + sceneName + " 不存在或未加载。");
        }
    }
}