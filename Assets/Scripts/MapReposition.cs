using UnityEngine;

public class MapReposition : MonoBehaviour
{
    public Transform player;  // Kéo Player vào Inspector
    public float moveDistance = 54f; // Khoảng cách cần di chuyển mỗi lần

    private Vector3 lastPlayerPos;

    private void Start()
    {
        lastPlayerPos = player.position; // Lưu vị trí ban đầu của Player
    }

    private void Update()
    {
        Vector3 diff = player.position - lastPlayerPos;

        if (Mathf.Abs(diff.x) >= moveDistance || Mathf.Abs(diff.y) >= moveDistance)
        {
            MoveMap(diff);
            lastPlayerPos = player.position; // Cập nhật vị trí sau khi di chuyển
        }
    }

    private void MoveMap(Vector3 direction)
    {
        float moveX = Mathf.Abs(direction.x) >= moveDistance ? Mathf.Sign(direction.x) * moveDistance : 0;
        float moveY = Mathf.Abs(direction.y) >= moveDistance ? Mathf.Sign(direction.y) * moveDistance : 0;

        transform.position += new Vector3(moveX, moveY, 0); // Di chuyển toàn bộ bản đồ
    }
}