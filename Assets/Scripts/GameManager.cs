using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime = 2 * 10f;  // 2*10f => 20s  || 5*60f => 6m

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
    }
    //ngoc add code
    public void GetExp()
    {
        exp++;
        if (exp >= nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
