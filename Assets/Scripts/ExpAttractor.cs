using UnityEngine;

public class ExpAttractor : MonoBehaviour
{
    private Transform target;
    public float attractSpeed = 10f;
    private bool isMovingToTarget = false;
    public float attractionRadius = 3f;

    // Thêm biến để xác định loại item
    private string itemType = "";

    void Awake()
    {
        // Xác định loại item dựa vào tên
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
        // Tự động thêm BoxCollider2D nếu chưa có
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.5f, 0.5f); // Kích thước mặc định
        }

        // Đảm bảo vật phẩm có tag
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Item";
        }

        // Set target to player if not already set
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            target = GameManager.instance.player.transform;
        }
        else
        {
            // Tìm player nếu không có reference
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

        // Chỉ di chuyển nếu đã bị kích hoạt bởi nam châm
        if (isMovingToTarget)
        {
            // Di chuyển vật phẩm về phía người chơi
            transform.position = Vector3.MoveTowards(transform.position, target.position, attractSpeed * Time.deltaTime);
        }
    }

    // Hàm này sẽ được gọi khi player nhặt được nam châm
    public void ForceAttract()
    {
        isMovingToTarget = true;
        // Tăng tốc độ hút khi bị buộc hút
        attractSpeed = 15f;
    }

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
    }

    // Xử lý thu thập vật phẩm khi chạm vào player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (GameManager.instance == null)
        {
            //Debug.LogError("GameManager.instance is null!");
            return;
        }

        // Xử lý theo loại item
        switch (itemType)
        {
            case "Exp":
                GameManager.instance.GetExp();
                //Debug.Log("Collected Exp through trigger");
                break;

            case "Gold":
                // Gọi trực tiếp phương thức và kiểm tra
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
                // Nếu là nam châm, kích hoạt chức năng hút
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

        // Hủy gameObject sau khi thu thập
        Destroy(gameObject);
    }
}
