using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("# Game Control")]
    public float GameTime;
    public bool isLive;

    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3,5,10,100,150,210,280,360,450,600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public GameObject uiResult;
    public GameObject enemyCleaner;

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
        
    }

    public void GameStart()
    {
        health = maxHealth;

        // Tìm lại player khi game restart
        if (player == null)
            player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        uiLevelUp.Select(0);
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.SetActive(true);
        Stop();
    }
    public void GameRetry()
    {
        Time.timeScale = 1; // Reset thời gian nếu bị pause
        Destroy(gameObject); // Xóa GameManager cũ để tránh lỗi
        SceneManager.LoadScene(0);
    }
    //ngoc add code
    public void GetExp()
    {
        if(!isLive) return;
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
