using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;
    Rigidbody2D Rigid;
     void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
    }

     public void Init(float damage,int per,Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        if(per >= 0)
        {
            Rigid.linearVelocity = dir * 15f; // Tốc độ đạn
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }
        per--;
        if(per < 0)
        {
            Rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100) return;
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
