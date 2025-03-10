using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    static bool lastMoveHorizontal = true;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Ngoc change code
        if (!collision.CompareTag("Area")) return;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;

        float dirX = diffX < 0 ? -1 : 1;
        float dirY = diffY < 0 ? -1 : 1;

        Vector3 playerDir = GameManager.instance.player.moveInput;
        switch (transform.tag)
        {
            case "Ground":
                if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
                {
                    transform.Translate(Vector3.right * dirX * 54);
                    lastMoveHorizontal = true; // Cập nhật hướng di chuyển gần nhất

                }
                else if (Mathf.Abs(diffX) < Mathf.Abs(diffY))
                {
                    transform.Translate(Vector3.up * dirY * 54);
                    lastMoveHorizontal = false; // Cập nhật hướng di chuyển gần nhất

                }
                else
                {
                    if (lastMoveHorizontal)
                    {
                        transform.Translate(Vector3.right * dirX * 54);
                    }
                    else
                    {
                        transform.Translate(Vector3.up * dirY * 54);
                    }
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
