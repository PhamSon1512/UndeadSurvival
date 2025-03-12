using UnityEngine;

public class ExpAttractor : MonoBehaviour
{
    private Transform target;
    public float attractSpeed = 10f;
    private bool isMovingToTarget = false;
    public float attractionRadius = 3f;
    private string itemType = "";

    void Awake()
    {
        if (gameObject.name.Contains("Exp"))
        {
            itemType = "Exp";
        }
        else if (gameObject.name.Contains("Gold"))
        {
            itemType = "Gold";
        }
        else if (gameObject.name.Contains("Mag"))
        {
            itemType = "Magnet";
        }
    }

    void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.5f, 0.5f);
        }

        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Item";
        }

        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            target = GameManager.instance.player.transform;
        }
        else
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isLive || target == null) return;

        if (isMovingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, attractSpeed * Time.deltaTime);
        }
    }
    
    // nam châm
    public void ForceAttract()
    {
        isMovingToTarget = true;
        attractSpeed = 15f;
    }

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
    }

    // thu thập vật phẩm
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (GameManager.instance == null)
        {
            //Debug.LogError("GameManager.instance is null!");
            return;
        }
        switch (itemType)
        {
            case "Exp":
                GameManager.instance.GetExp();
                //Debug.Log("Collected Exp through trigger");
                break;

            case "Gold":
                if (GameManager.instance != null)
                {
                    GameManager.instance.GoldCount();
                    //Debug.Log("Collected Gold through trigger. Gold count: " + GameManager.instance.gold);
                }
                else
                {
                    //Debug.LogError("GameManager is null when collecting gold");
                }
                break;

            case "Magnet":
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.ActivateMagnet();
                }
                //Debug.Log("Collected Magnet through trigger");
                break;

            default:
                //Debug.Log("Collected unknown item: " + gameObject.name);
                break;
        }
        Destroy(gameObject);
    }
}
