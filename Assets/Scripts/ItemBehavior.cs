using System.Collections;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public float itemDisappearTime = 10f;
    private bool isCollected = false;
    private string itemName;

    void Start()
    {
        StartCoroutine(ItemDisappearTimer());
        itemName = gameObject.name.Replace("(Clone)", "").Trim();

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive) return;

        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            if (itemName.Contains("Exp"))
            {
                GameManager.instance.GetExp();
            }
            else if (itemName.Contains("Mag"))
            {
                AttractionMagnet();
            }

            Destroy(gameObject);
        }
    }

    void AttractionMagnet()
    {
        // Find all experience items and make them move toward the player
        GameObject[] allItems = Object.FindObjectsOfType<GameObject>(true);
        foreach (GameObject item in allItems)
        {
            if (item != null && item.name.Contains("Exp"))
            {
                ExpAttractor attractor = item.GetComponent<ExpAttractor>();
                if (attractor == null)
                {
                    attractor = item.AddComponent<ExpAttractor>();
                }
                attractor.SetTarget(GameManager.instance.player.transform);
            }
        }
    }

    IEnumerator ItemDisappearTimer()
    {
        yield return new WaitForSeconds(itemDisappearTime);
        if (gameObject != null && !isCollected)
        {
            Destroy(gameObject);
        }
    }
}
