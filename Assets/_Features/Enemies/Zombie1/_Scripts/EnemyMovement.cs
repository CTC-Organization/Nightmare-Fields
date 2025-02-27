using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    public IAstarAI ai;
    private EnemyController ec;

    private void Start()
    {
        ec = GetComponent<EnemyController>();
        ai = GetComponent<IAstarAI>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            SetTarget(player.transform);
        }

        if (Random.Range(0, 100) > 95)
        { // sorteio mini modo hunter
            ai.maxSpeed *= 2;
            GetComponent<EnemyHealth>().startingHealth *= 1.2f;
        }
    }

    private void Update()
    {
        if (ec.isDying && !ai.isStopped)
        {

            ai.destination = transform.position;
            ai.isStopped = true; // Para a IA
            if (TryGetComponent<BoxCollider>(out var collider))
            {
                collider.enabled = false;
            }
            return;
        }
        else if (ec.isDying)
        {
            return;
        }
        ec.UpdateAnimatorParameters(ai.velocity);
    }

    public void SetTarget(Transform target)
    {
        if (aiDestinationSetter != null)
        {
            aiDestinationSetter.target = target;
        }
    }
}
