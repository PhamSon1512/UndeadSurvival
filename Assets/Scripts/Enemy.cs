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
    bool isBoss;
    [Header("Drop Settings")]
    public GameObject[] possibleDrops; // Array of possible item prefabs (Exp 2 and Mag)
    [Range(0f, 1f)]
    public float dropChance = 0.1f;    // 10% chance to drop an item by default
    public float itemDisappearTime = 10f; // How long the item stays in the world

    bool isLive;
    WaitForFixedUpdate Wait;
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D collider2D;

    private void Awake()
    {
        isBoss = false;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Wait = new WaitForFixedUpdate();
        collider2D = GetComponent<Collider2D>();
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
        if(maxHealth >= 500)
        {
            isBoss = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive) return;

        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        Bullet bullet = collision.GetComponent<Bullet>();
        health -= bullet.damage;
        StartCoroutine(Knokback());

        if (health <= 0)
        {
            // Die
            isLive = false;
            collider2D.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            animator.SetBool("Dead", true);
            if(isBoss == true)
            {
                Spawn.Instance.BossDead();
            }
            // Drop items directly instead of treasure chest
            DropItem();
            Spawn.Instance.reducenumberofenemy();
            StartCoroutine(Dead());
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void DropItem()
    {
        // Check if we should drop anything based on drop chance
        if (Random.value <= dropChance && possibleDrops != null && possibleDrops.Length > 0)
        {
            // Select a random item from the possible drops
            int randomIndex = Random.Range(0, possibleDrops.Length);
            GameObject itemPrefab = possibleDrops[randomIndex];

            if (itemPrefab != null)
            {
                // Spawn the item slightly above the enemy position
                Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

                // Add ExpAttractor component and set target to player
                ExpAttractor attractor = spawnedItem.GetComponent<ExpAttractor>();
                if (attractor == null)
                {
                    attractor = spawnedItem.AddComponent<ExpAttractor>();
                }
                attractor.SetTarget(GameManager.instance.player.transform);

                // Add ItemBehavior component if it doesn't exist
                ItemBehavior itemBehavior = spawnedItem.GetComponent<ItemBehavior>();
                if (itemBehavior == null)
                {
                    itemBehavior = spawnedItem.AddComponent<ItemBehavior>();
                }

                // Set the disappear time
                itemBehavior.itemDisappearTime = itemDisappearTime;
            }
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