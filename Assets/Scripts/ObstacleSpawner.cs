using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Danh sách obstacle có thể spawn
    public GameObject player; // Người chơi
    public float spawnDistance = 15f; // Khoảng cách tối đa để spawn
    public float spawnInterval = 10f; // Khoảng cách giữa các obstacle
    public float despawnDistance = 100f; // Khoảng cách để xóa obstacle
    public float spawnRangeX = 10f; // Phạm vi spawn theo trục X
    public float emptyChance = 40f; // Xác suất để khoảng trống (40%)

    private float nextSpawnY; // Vị trí Y để spawn tiếp theo
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        nextSpawnY = player.transform.position.y + 10f;

        // Spawn trước một số obstacle để game có chướng ngại vật ban đầu
        for (int i = 0; i < 3; i++)
        {
            SpawnObstacle();
        }
    }

    void Update()
    {
        if (player.transform.position.y + spawnDistance > nextSpawnY)
        {
            SpawnObstacle();
        }

        RemoveFarObstacles();
    }

    void SpawnObstacle()
    {
        // Xác suất tạo khoảng trống để người chơi có đường đi
        if (Random.value < emptyChance)
        {
            Debug.Log("Empty space at Y = " + nextSpawnY);
            nextSpawnY += spawnInterval;
            return;
        }

        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), nextSpawnY, 0);
        GameObject newObstacle = Instantiate(obstaclePrefabs[randomIndex], spawnPosition, Quaternion.identity);

        spawnedObstacles.Add(newObstacle);
        nextSpawnY += spawnInterval;

        Debug.Log("Spawned obstacle at Y = " + nextSpawnY);
    }

    void RemoveFarObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i].transform.position.y < player.transform.position.y - despawnDistance)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }
}
