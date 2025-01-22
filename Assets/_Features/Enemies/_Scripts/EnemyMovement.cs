using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    private IAstarAI ai;

    private void Start()
    {
        ai = GetComponent<IAstarAI>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            SetTarget(player.transform);
        }
    }

    private void Update()
    {
        UpdateAnimatorParameters();
    }

    public void SetTarget(Transform target)
    {
        if (aiDestinationSetter != null)
        {
            aiDestinationSetter.target = target;
        }
    }

    private void UpdateAnimatorParameters()
    {
        Vector3 velocity = ai.velocity;
        animator.SetFloat("moveSpeed", velocity.magnitude);

        if (velocity.magnitude > 0.1f)
        {
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);
        }
    }
}
