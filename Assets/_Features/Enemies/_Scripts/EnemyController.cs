using Pathfinding;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AIPath aIPath;
    Vector2 direction;

    private void Start()
    {

    }

    private void Update()
    {
        faceVelocity();
    }

    void faceVelocity()
    {
        direction = aIPath.desiredVelocity;
        transform.right = direction;
    }
}
