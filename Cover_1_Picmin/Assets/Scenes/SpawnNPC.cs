using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    
    public GameObject[] npcPrefabs; // NPC预制体数组
    public int npcCount = 10; // NPC数量
    public Transform spawnArea; // NPC生成区域

    void Start()
    {
        // 获取选择的角色索引
        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", -1);
        
        {
            
            SpawnNPCs(); // 生成 NPC
        }
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < npcCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                0,
                Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2, spawnArea.position.z + spawnArea.localScale.z / 2)
            );

            // 随机选择 NPC 预制体
            int npcIndex = Random.Range(0, npcPrefabs.Length);
            Instantiate(npcPrefabs[npcIndex], randomPosition, Quaternion.identity);
        }
    }
}