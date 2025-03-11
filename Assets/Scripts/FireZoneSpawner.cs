using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZoneSpawner : MonoBehaviour
{
    public GameObject fireZonePrefab; // Prefab của FireZone
    public Transform player; // Người chơi
    public float spawnRadius = 10f; // Bán kính spawn tối đa
    public float minSpawnDistance = 3f; // Khoảng cách tối thiểu từ người chơi
    public float fireZoneLifetime = 8f; // Thời gian tồn tại của mỗi FireZone
    public float spawnInterval = 5f; // Thời gian giữa mỗi lần spawn
    public int maxFireZones = 3; // Số lượng FireZone tối đa cùng lúc

    private List<GameObject> activeFireZones = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnFireZones());
    }

    IEnumerator SpawnFireZones()
    {
        while (true)
        {
            if (activeFireZones.Count < maxFireZones)
            {
                SpawnFireZone();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnFireZone()
    {
        if (player == null || fireZonePrefab == null) return;

        Vector2 spawnPos;
        int attempts = 0;

        do
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomDistance = Random.Range(minSpawnDistance, spawnRadius);
            spawnPos = (Vector2)player.position + new Vector2(
                Mathf.Cos(randomAngle) * randomDistance,
                Mathf.Sin(randomAngle) * randomDistance
            );

            attempts++;
            if (attempts > 20) break; // Tránh vòng lặp vô hạn

        } while (Vector2.Distance(player.position, spawnPos) < minSpawnDistance);

        GameObject newFireZone = Instantiate(fireZonePrefab, spawnPos, Quaternion.identity);
        Debug.Log("FireZone spawned at: " + spawnPos);
        activeFireZones.Add(newFireZone);

        StartCoroutine(DestroyFireZoneAfterTime(newFireZone, fireZoneLifetime));
    }

    IEnumerator DestroyFireZoneAfterTime(GameObject fireZone, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (fireZone != null)
        {
            activeFireZones.Remove(fireZone);
            Destroy(fireZone);
        }
    }
}
