using UnityEngine;
using UnityEngine.Rendering;

public class Spawn : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public static Spawn Instance;
    public float timerspawnBoss = 30f;
    float timer;
    float timerBoss;
    public float levelTime;
    int numberofenemy;
    int maxenemy;
    int level;
    bool isBoss;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        spawnPoint = GetComponentsInChildren<Transform>();
        numberofenemy = 0;
        maxenemy = 20;
        isBoss = false;
        //levelTime = GameManager.instance.MaxGameTime / spawnData.Length;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive) return;
        if(!isBoss)
            timerBoss += Time.deltaTime;
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.GameTime / 10f);
        //ngoc add level restriction
        if (level >= spawnData.Length)
        {
            level = spawnData.Length - 1;
        }
        //Debug.Log(timerBoss+"    "+ isBoss);
        if(timerBoss >= timerspawnBoss)
        {
            SpawnsBoss();
            isBoss = true;
            timerBoss = 0;
        }
        if (timer > spawnData[level].time)
        {
            //numberofenemy += 1;
            
            timer = 0;
            if (numberofenemy < maxenemy)
            {
                numberofenemy += 1;
                Spawns();
            }
        }
    }
    void Spawns()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[Random.Range(0, spawnData.Length - 1)]);
    }
    void SpawnsBoss()
    {
        GameObject enemyx = GameManager.instance.pool.Get(0);
        enemyx.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemyx.GetComponent<Enemy>().Init(spawnData[spawnData.Length - 1]);
        enemyx.transform.localScale = new Vector3(2, 2, 2);
    }
    public void reducenumberofenemy()
    {
        numberofenemy = numberofenemy - 1;
        //Debug.Log(numberofenemy);
    }
    public void Increasenumberofenemy()
    {
        maxenemy = maxenemy + 10;
       
    }
    public void BossDead()
    {
        Debug.Log("Boss Dead");
        isBoss = false;
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
