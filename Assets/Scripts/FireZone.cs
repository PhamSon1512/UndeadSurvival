using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float damagePerSecond = 50000f; // Lượng sát thương mỗi giây
    private bool isPlayerInside = false;
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && player != null && GameManager.instance.isLive)
        {
            // Tăng sát thương theo cấp độ của người chơi
            float levelMultiplier = 1 + (GameManager.instance.level * 0.5f);
            float actualDamage = damagePerSecond * levelMultiplier * Time.deltaTime;

            GameManager.instance.health -= actualDamage;
            Debug.Log("Player health: " + GameManager.instance.health + " (Damage: " + actualDamage + ")");
        }
    }

}
