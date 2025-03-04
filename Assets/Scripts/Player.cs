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

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hands = GetComponentsInChildren<Hand>(true);
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
        if(!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health < 0)
        {
            for(int i =2;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
