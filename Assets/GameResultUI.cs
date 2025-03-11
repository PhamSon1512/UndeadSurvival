using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResultUI : MonoBehaviour
{
    public GameObject gameOverPanel;  // Panel chứa thông số tổng kết
    public GameObject textDead;       // Text "You Died"
    public Button retryButton;        // Button Retry

    public Text killsText;
    public Text timeText;
    public Text goldText;
    public Text highestGoldText;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Ẩn tổng kết ban đầu
        textDead.SetActive(false);      // Ẩn text "Dead" ban đầu
        retryButton.gameObject.SetActive(false); // Ẩn nút Retry ban đầu
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        textDead.SetActive(true);       // Hiện "You Died"
        retryButton.gameObject.SetActive(true); // Hiện nút Retry

        int totalKills = GameManager.instance.kill;
        int goldEarned = GameManager.instance.gold;
        float totalTime = GameManager.instance.GameTime;

        // Cập nhật UI
        killsText.text = "Total Kills: " + totalKills;
        timeText.text = string.Format("Total Time: {0:D2}:{1:D2}", (int)totalTime / 60, (int)totalTime % 60);
        goldText.text = "Gold Earned: " + goldEarned;

        // Cập nhật Highest Gold Record
        int highestGold = PlayerPrefs.GetInt("HighestGold", 0);
        if (goldEarned > highestGold)
        {
            highestGold = goldEarned;
            PlayerPrefs.SetInt("HighestGold", highestGold);
            PlayerPrefs.Save();
        }

        highestGoldText.text = "Highest Gold Record: " + highestGold;
    }
}
