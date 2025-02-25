using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public PoolManager pool;
    public Player player;

    public int level;
    public int kill;
    public float exp;

    public float GameTime;
    public float MaxGameTime = 2 * 10f;  // 2*10f => 20s  || 5*60f => 6m
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
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player == null)
                Debug.LogError("Player not found!");
        }
    }
}
