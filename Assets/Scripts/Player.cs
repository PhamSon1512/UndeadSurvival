using System.Runtime.ExceptionServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Animator animator;
    public Vector3 moveInput;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    public Scanner scanner;
    public Hand[] hands;

    // Thêm biến kiểm soát nam châm
    public bool hasMagnet = false;
    public float magnetDuration = 10f;
    private float magnetTimer = 0f;

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hands = GetComponentsInChildren<Hand>(true);

        // Đảm bảo Player có tag
        gameObject.tag = "Player";

        // Kiểm tra và thêm Collider2D nếu chưa có
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        // Xử lý di chuyển
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        Vector3 movement = moveInput.normalized;
        transform.position += moveInput * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        // Xử lý thời gian nam châm
        if (hasMagnet)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0)
            {
                hasMagnet = false;
                Debug.Log("Magnet effect ended");
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        Vector2 movement = moveInput.normalized * moveSpeed;
        rb.linearVelocity = movement;
        if (moveInput.magnitude == 0)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            return;
        }
        if (!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime * 10;
        if (GameManager.instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    // Thêm phương thức kích hoạt nam châm
    public void ActivateMagnet()
    {
        hasMagnet = true;
        magnetTimer = magnetDuration;

        // Tìm tất cả ExpAttractor trong scene
        ExpAttractor[] allAttractors = FindObjectsOfType<ExpAttractor>();

        // Kích hoạt hút cho tất cả
        foreach (ExpAttractor attractor in allAttractors)
        {
            attractor.ForceAttract();
        }

        Debug.Log("Found and activated " + allAttractors.Length + " attractors");
    }
}
