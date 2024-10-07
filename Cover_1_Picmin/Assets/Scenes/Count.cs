using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public string targetTag = "Target"; // 目标物体的标签
    public string npcTag = "NPC"; // NPC 的标签
    public TextMeshProUGUI counterText; // TextMeshPro 计数器文本
    public TextMeshProUGUI timerText; // TextMeshPro 计时器文本
    private int count = 0; // 重合计数
    private const int targetCount = 6; // 目标数量
    private float countdownTime = 100f; // 倒计时初始时间

    private void Start()
    {
        // 初始化计数器文本
        UpdateCounterText();
        UpdateTimerText();
    }

    private void Update()
    {
        // 每帧检查重合
        CheckOverlap();
        UpdateTimer(); // 更新计时器
    }

    private void CheckOverlap()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag); // 查找所有 NPC
        count = 0; // 重置计数

        foreach (var npc in npcs)
        {
            // 查找所有目标
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (var target in targets)
            {
                // 使用 Collider 碰撞检测
                if (AreColliding(npc, target))
                {
                    count++;
                }
            }
        }

        // 更新计数器文本
        UpdateCounterText();

        // 检查是否达到 6/6
        if (count >= targetCount)
        {
            SceneManager.LoadScene("Main_2"); // 切换成功场景
        }
    }

    private bool AreColliding(GameObject npc, GameObject target)
    {
        Collider npcCollider = npc.GetComponent<Collider>();
        Collider targetCollider = target.GetComponent<Collider>();

        if (npcCollider != null && targetCollider != null)
        {
            // 检查是否重叠
            return npcCollider.bounds.Intersects(targetCollider.bounds);
        }
        return false;
    }

    private void UpdateCounterText()
    {
        // 更新计数器文本
        counterText.text = $"Count: {count}/{targetCount}"; // 显示当前计数
    }

    private void UpdateTimer()
    {
        // 减少倒计时时间
        countdownTime -= Time.deltaTime;

        // 更新计时器文本
        UpdateTimerText();

        // 检查计时器是否到达 0
        if (countdownTime <= 0)
        {
            SceneManager.LoadScene("FailureScene"); // 切换失败场景
        }
    }

    private void UpdateTimerText()
    {
        // 更新计时器文本
        timerText.text = $"Time: {Mathf.Ceil(countdownTime)}"; // 向上取整显示整数
    }
}