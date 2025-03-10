using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles; // Danh sách prefab cây
    public Transform player; // Người chơi
    public float spawnRadius = 12f; // Bán kính spawn cây
    public float despawnDistance = 20f; // Khoảng cách tối đa trước khi xóa
    public float gridSize = 3f; // Khoảng cách giữa các cây
    public float spawnChance = 0.3f; // Xác suất spawn tại một điểm
    public int maxObstacles = 50; // Số cây tối đa trên bản đồ
    public float checkInterval = 1f; // Thời gian giữa mỗi lần kiểm tra (giảm tần suất tính toán)

    private Dictionary<Vector2, GameObject> spawnedObstacles = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        StartCoroutine(ManageObstacles()); // Chạy kiểm tra mỗi giây
    }

    IEnumerator ManageObstacles()
    {
        while (true)
        {
            SpawnObstacles();
            DespawnObstacles();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void SpawnObstacles()
    {
        if (player == null || obstacles.Count == 0 || spawnedObstacles.Count >= maxObstacles) return;

        for (float x = -spawnRadius; x <= spawnRadius; x += gridSize)
        {
            for (float y = -spawnRadius; y <= spawnRadius; y += gridSize)
            {
                if (spawnedObstacles.Count >= maxObstacles) return; // Dừng ngay nếu đủ số cây tối đa

                Vector2 spawnPos = new Vector2(
                    Mathf.Round(player.position.x + x),
                    Mathf.Round(player.position.y + y)
                );

                float distance = Vector2.Distance(player.position, spawnPos);

                if (!spawnedObstacles.ContainsKey(spawnPos) &&
                    distance > spawnRadius * 0.6f &&
                    Random.value < spawnChance)
                {
                    int obstacleIndex = Random.Range(0, obstacles.Count);
                    GameObject newTree = Instantiate(obstacles[obstacleIndex], spawnPos, Quaternion.identity);
                    spawnedObstacles[spawnPos] = newTree;
                }
            }
        }
    }

    void DespawnObstacles()
    {
        List<Vector2> toRemove = new List<Vector2>();

        foreach (var kvp in spawnedObstacles)
        {
            if (Vector2.Distance(player.position, kvp.Key) > despawnDistance)
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
        {
            spawnedObstacles.Remove(key);
        }
    }
}
