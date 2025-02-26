using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // Khoảng cách bắn
    public LayerMask layerMask;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, layerMask);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;
        foreach(RaycastHit2D target in targets)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
    void Start()
    {
        
    }

    

    void Update()
    {
        
    }
}
