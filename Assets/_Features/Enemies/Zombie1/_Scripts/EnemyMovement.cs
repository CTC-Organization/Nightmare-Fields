using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    private IAstarAI ai;
    public bool isDying = false;

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
        if (isDying && !ai.isStopped)
        {

            ai.destination = transform.position;
            ai.isStopped = true; // Para a IA
            if (TryGetComponent<BoxCollider>(out var collider))
            {
                collider.enabled = false;
            }
            return;
        }
        else if (isDying)
        {
            return;
        }
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
