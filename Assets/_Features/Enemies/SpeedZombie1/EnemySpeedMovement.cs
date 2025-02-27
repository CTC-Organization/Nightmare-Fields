using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemySpeedMovement : MonoBehaviour
{
    private AIDestinationSetter aiDestinationSetter;
    private IAstarAI ai;
    private EnemyController ec;
    private Rigidbody2D rb;
    [SerializeField] private TrailRenderer trr;

    [SerializeField] private TrailRenderer trl;

    [Header("Dash config")]
    private float dashingPower = 24f;
    private float dashingTime = 0.25f;
    private float dashingCooldown = 3f;
    private bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ec = GetComponent<EnemyController>();
        ai = GetComponent<IAstarAI>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            SetTarget(player.transform);
            StartCoroutine(Dash());
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
        // if (ec.isDashing) return;
        ec.UpdateAnimatorParameters(ai.velocity);
    }

    public void SetTarget(Transform target)
    {
        if (aiDestinationSetter != null)
        {
            aiDestinationSetter.target = target;
        }
    }


    private IEnumerator Dash()
    {
        while (true)
        {
            yield return new WaitUntil(() => canDash);

            canDash = false;
            // ec.isDashing = true;
            // ec.UpdateAnimatorParameters(Vector3.zero); // para anim de movimento

            Vector3 dashDirection = (aiDestinationSetter.target.position - transform.position).normalized;

            // Se o jogador não está se movendo, usa a direção do eixo X
            if (dashDirection == Vector3.zero)
            {
                dashDirection = Vector3.right;
            }
            trl.emitting = true;
            trr.emitting = true;
            rb.linearVelocity = dashDirection * dashingPower;


            yield return new WaitForSeconds(dashingTime);


            rb.linearVelocity = Vector3.zero;
            trl.emitting = false;
            trr.emitting = false;

            // ec.isDashing = false; // retorna animação de movimento

            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }
    }
}
