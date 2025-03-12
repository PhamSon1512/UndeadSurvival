using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZoneSpawner : MonoBehaviour
{
    public GameObject fireZonePrefab; 
    public Transform player; 
    public float spawnRadius = 10f; 
    public float minSpawnDistance = 3f; 
    public float fireZoneLifetime = 8f; 
    public float spawnInterval = 3f;
    public int maxFireZones = 3; 

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
        
        maxFireZones = 3 + GameManager.instance.level*3;

        if (activeFireZones.Count >= maxFireZones) return;

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
