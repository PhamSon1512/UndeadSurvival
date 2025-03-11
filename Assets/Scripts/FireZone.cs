using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float damagePerSecond = 10f; // Lượng sát thương mỗi giây
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
            GameManager.instance.health -= damagePerSecond * Time.deltaTime;
        }
    }
}
