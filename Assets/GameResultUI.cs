using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameResultUI : MonoBehaviour
{
    public GameObject gameOverPanel;  // Panel chứa thông số tổng kết
    public GameObject textDead;       // Text "You Died"
    public Button retryButton;        // Button Retry
    public Text killsText;
    public Text timeText;
    public Text goldText;
    public Text highestKillText;
    public Button goldRevivalButton;
    public Button adRevivalButton;
    public Text goldRevivalText;

    // Constants
    private const int REVIVAL_GOLD_COST = 20;

    // Revival tracker
    private bool hasRevivedThisSession = false;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Ẩn tổng kết ban đầu
        textDead.SetActive(false);      // Ẩn text "Dead" ban đầu
        retryButton.gameObject.SetActive(false); // Ẩn nút Retry ban đầu

        if (goldRevivalButton != null)
        {
            goldRevivalButton.onClick.AddListener(ReviveWithGold);
            goldRevivalButton.gameObject.SetActive(false);
        }

        if (adRevivalButton != null)
        {
            adRevivalButton.onClick.AddListener(ReviveWithAd);
            adRevivalButton.gameObject.SetActive(false);
        }
        LoadPersistentGold();
    }

    // hiển thị vàng
    private void LoadPersistentGold()
    {
        if (GameManager.instance != null && GameManager.instance.gold == 0)
        {
            int savedGold = PlayerPrefs.GetInt("PersistentGold", 0);
            GameManager.instance.gold = savedGold;
            Debug.Log("Loaded persistent gold: " + savedGold);
        }
    }

    // lưu vàng thu được
    private void SavePersistentGold()
    {
        if (GameManager.instance != null)
        {
            PlayerPrefs.SetInt("PersistentGold", GameManager.instance.gold);
            PlayerPrefs.Save();
            Debug.Log("Saved persistent gold: " + GameManager.instance.gold);
        }
    }

    // game over panel
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        textDead.SetActive(true);

        int totalKills = GameManager.instance.kill;
        int goldEarned = GameManager.instance.gold;
        float totalTime = GameManager.instance.GameTime;

        killsText.text = "Total Kills: " + totalKills;
        timeText.text = string.Format("Total Time: {0:D2}:{1:D2}", (int)totalTime / 60, (int)totalTime % 60);
        goldText.text = "Gold Earned: " + goldEarned;

        int highestKill = PlayerPrefs.GetInt("HighestKill", 0);
        if (totalKills > highestKill)
        {
            highestKill = totalKills;
            PlayerPrefs.SetInt("HighestKill", highestKill);
        }
        highestKillText.text = "Highest Kill Record: " + highestKill;

        if (!hasRevivedThisSession)
        {
            ShowRevivalOptions();
        }
        else
        {
            retryButton.gameObject.SetActive(true);
        }
        SavePersistentGold();
    }

    private void ShowRevivalOptions()
    {
        bool hasEnoughGold = GameManager.instance.gold >= REVIVAL_GOLD_COST;

        if (goldRevivalButton != null)
        {
            goldRevivalButton.gameObject.SetActive(true);
            goldRevivalButton.interactable = hasEnoughGold;

            if (goldRevivalText != null)
            {
                goldRevivalText.text = "Revive (" + REVIVAL_GOLD_COST + " Gold)";
                goldRevivalText.color = hasEnoughGold ? Color.blue : Color.red;
            }
        }

        if (adRevivalButton != null)
        {
            adRevivalButton.gameObject.SetActive(true);
        }

        retryButton.gameObject.SetActive(true);
    }

    public void ReviveWithGold()
    {
        if (GameManager.instance.gold >= REVIVAL_GOLD_COST)
        {
            GameManager.instance.gold -= REVIVAL_GOLD_COST;
            SavePersistentGold();
            RevivePlayer();
        }
        else
        {
            Debug.Log("Not enough gold to revive!");
        }
    }

    public void ReviveWithAd()
    {
        StartCoroutine(ShowAdAndRevive());
    }

    private IEnumerator ShowAdAndRevive()
    {
        Debug.Log("Showing advertisement...");

        if (goldRevivalButton != null) goldRevivalButton.interactable = false;
        if (adRevivalButton != null) adRevivalButton.interactable = false;
        if (retryButton != null) retryButton.interactable = false;

        yield return new WaitForSecondsRealtime(2f);

        Debug.Log("Ad finished, reviving player");

        if (goldRevivalButton != null) goldRevivalButton.interactable = true;
        if (adRevivalButton != null) adRevivalButton.interactable = true;
        if (retryButton != null) retryButton.interactable = true;

        RevivePlayer();
    }

    private void RevivePlayer()
    {
        hasRevivedThisSession = true;
        gameOverPanel.SetActive(false);
        textDead.SetActive(false);
        Player player = GameManager.instance.player;
        if (player != null)
        {
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Stand");
            }
            else
            {
                Debug.LogWarning("Animator not found on Player!");
            }
            for (int i = 2; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(true);
            }
            GameManager.instance.health = GameManager.instance.maxHealth * 0.5f;
            StartCoroutine(ProvideTemporaryInvincibility(player));
        }
        else
        {
            Debug.LogError("Player is null! Make sure it's assigned in GameManager.");
        }
        if (goldRevivalButton != null) goldRevivalButton.gameObject.SetActive(false);
        if (adRevivalButton != null) adRevivalButton.gameObject.SetActive(false);
        GameManager.instance.Resume();
    }

    // layer Invincible after revive
    private IEnumerator ProvideTemporaryInvincibility(Player player)
    {
        player.gameObject.layer = 3;
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            float endTime = Time.time + 3f;
            while (Time.time < endTime)
            {
                playerSprite.color = new Color(1f, 1f, 1f, 0.5f);
                yield return new WaitForSeconds(0.1f);
                playerSprite.color = new Color(1f, 1f, 1f, 1f);
                yield return new WaitForSeconds(0.1f);
            }
            playerSprite.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        player.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
