using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isLive = true;
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!isLive) return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isLive) return;
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
}
