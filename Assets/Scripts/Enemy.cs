using System.Collections;
using UnityEngine;
using static Spawn;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animatorControllers;
    public Rigidbody2D target;

    bool isLive;

    WaitForFixedUpdate Wait;
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D collider2D;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Wait = new WaitForFixedUpdate();
        collider2D = GetComponent<Collider2D>();
        //isLive = true;
    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        collider2D.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        animator.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animatorControllers[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive) return;
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")||!isLive) return;
        
            Bullet bullet = collision.GetComponent<Bullet>();
            health -= bullet.damage;
            StartCoroutine(Knokback());
            if (health <= 0)
            {
                //die
                isLive = false;
                //GameManager.instance.AddScore(1);
                collider2D.enabled = false;
                rigid.simulated = false;
                spriteRenderer.sortingOrder = 1;
                animator.SetBool("Dead", true);
                StartCoroutine(Dead());
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
            }
            else
            {
                animator.SetTrigger("Hit");
                //live
            }
        
    }

    IEnumerator Knokback()
    {
        yield return Wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

    }
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}


