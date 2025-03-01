using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime = 2 * 10f;  // 2*10f => 20s  || 5*60f => 6m
    public bool isLive;

    [Header("# Player Control")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3,5,10,100,150,210,280,360,450,600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isLive) return;
        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
        }
    }
    [System.Obsolete]
    private void Start()
    {
        health = maxHealth;
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player == null)
                Debug.LogError("Player not found!");
        }

        //Son add code
        uiLevelUp.Select(0);
    }
    //ngoc add code
    public void GetExp()
    {
        exp++;
        if (exp >= nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            // Son add code
            uiLevelUp.Show();
        }
    }

    //Son add code
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
