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

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        Vector3 movement = moveInput.normalized;
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = moveInput.normalized * moveSpeed;
        rb.linearVelocity = movement;

        if (moveInput.magnitude == 0)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
