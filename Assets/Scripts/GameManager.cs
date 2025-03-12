using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int gold;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public GameResultUI gameResultUI;
    // Thêm tham chiếu đến UI
    [Header("# UI References")]
    public GameObject goldCountUI;
    public Text persistentGoldText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPersistentGold();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Đảm bảo các giá trị ban đầu
        if (health <= 0) health = maxHealth;
        if (gold < 0) gold = 0;
        UpdatePersistentGoldDisplay();
    }

    private void Update()
    {
        if (!isLive) return;
        GameTime += Time.deltaTime;
    }

    // display gold currenly
    private void LoadPersistentGold()
    {
        gold = PlayerPrefs.GetInt("PersistentGold", 0);
        Debug.Log("Loaded persistent gold: " + gold);
    }

    // save gold
    private void SavePersistentGold()
    {
        PlayerPrefs.SetInt("PersistentGold", gold);
        PlayerPrefs.Save();
        Debug.Log("Saved persistent gold: " + gold);
    }

    // display gold after collection
    private void UpdatePersistentGoldDisplay()
    {
        if (persistentGoldText != null)
        {
            persistentGoldText.text = "Gold: " + gold.ToString();
        }
    }

    public void GameStart()
    {
        health = maxHealth;
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

    public void GoldCount()
    {
        if (!isLive) return;
        gold++;
        SavePersistentGold();

        Debug.Log("GoldCount called - Current gold: " + gold);

        NewMonoBehaviourScript[] uiScripts = FindObjectsOfType<NewMonoBehaviourScript>();
        foreach (var script in uiScripts)
        {
            if (script.type == NewMonoBehaviourScript.InfoType.Gold)
            {
                Text goldText = script.GetComponent<Text>();
                if (goldText != null)
                {
                    goldText.text = gold.ToString();
                    Debug.Log("Gold UI updated to: " + gold);
                }
            }
        }
    }

    public void KillCount()
    {
        if (!isLive) return;
        kill++;
        Debug.Log("Enemy killed! Total kills: " + kill);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        if (gameResultUI != null)
        {
            gameResultUI.ShowGameOverPanel();
        }
        Stop();
    }

    public void GameRetry()
    {
        Time.timeScale = 1; // Reset thời gian nếu bị pause
        SavePersistentGold();
        Destroy(gameObject); // Xóa GameManager cũ để tránh lỗi
        SceneManager.LoadScene(0); // Load lại scene
    } 

    public void GetExp()
    {
        if (!isLive) return;
        exp++;

        // Kiểm tra lên cấp
        if (exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
            Debug.Log("Level up! Current level: " + level);
        }
    }

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
