using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    float timer;

    int level;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive) return;
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.GameTime / 10f);
        //ngoc add level restriction
        if (level >= spawnData.Length)
        {
            level = spawnData.Length - 1;
        }
        if (timer > spawnData[level].time)
        {
           // numberofenemy += 1;
            timer = 0;
            Spawns();
        }
    }
    void Spawns()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);

    }
    [System.Serializable]
    public class SpawnData
    {
        public int spriteType;
        public float time;
        public float health;
        public float speed;
    }
}
