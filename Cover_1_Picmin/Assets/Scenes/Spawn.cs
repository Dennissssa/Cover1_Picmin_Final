using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] characterPrefabs; // 角色预制体数组

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", -1); // 获取选择的角色索引
        if (index >= 0 && index < characterPrefabs.Length)
        {
            // 生成选择的角色
            Instantiate(characterPrefabs[index], Vector3.zero, Quaternion.identity);
        }
    }
}