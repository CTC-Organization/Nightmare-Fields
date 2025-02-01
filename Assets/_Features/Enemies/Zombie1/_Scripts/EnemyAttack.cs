using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector3 hitBoxSize;
    [SerializeField] private float attackStartDistance;
    [SerializeField] private Animator animator;
    private Coroutine attackRoutine = null;

    [SerializeField] private Vector3 attackOrigin; // Dist�ncia de ataque


    private float lastAttackTime;
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    private void Update()
    {

        if (GetComponent<EnemyMovement>().isDying)
        {
            if (attackRoutine != null) StopAllCoroutines();
            return;
        }
        else if (target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (target != null)
        {

            Gizmos.DrawWireCube(attackOrigin, hitBoxSize);
        }
        else
        {
            Vector3 directionStart = attackOrigin * attackStartDistance;

            Gizmos.DrawWireCube(directionStart, hitBoxSize);
        }

    }

    /// <summary>
    /// Realiza o ataque do inimigo ao atingir o alvo e controla o cooldown entre ataques
    /// </summary>
    /// <returns>Retorna uma Coroutine que aguarda o cooldown do ataque</returns>
    IEnumerator Attack()
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;


        attackOrigin = transform.position + directionToTarget * attackStartDistance;

        animator.SetTrigger("Attack");

        LayerMask targetMask = 1 << target.layer;
        HitType hitType = HitBoxCreate(targetMask, attackOrigin);

        // Espera o tempo do cooldown antes de permitir um novo ataque
        lastAttackTime = Time.time;
        switch (hitType)
        {
            case HitType.Hit:
                {
                    target.GetComponent<Health>().TakeEnemyAttackDamage(hitType, damage);
                    float targetHealth = target.GetComponent<Health>().currentHealth;
                    Debug.Log($"Dano do inimigo: {damage}\n HP do player = {targetHealth}");
                }
                break;
            case HitType.Miss:
                {
                    Debug.Log($"Ataque do inimigo " + name + "falhou");
                }
                break;
            case HitType.Invulnerable:
                {
                    Debug.Log($"Player está invencivel, ataque do inimigo falhou");
                }
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(attackCooldown); // aguarda  cooldown
    }


    HitType HitBoxCreate(LayerMask targetMask, Vector3 attackOrigin) // ataque "Realista" pode errar ou acertar
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackOrigin, new Vector2(hitBoxSize.x, hitBoxSize.y), 0);
        Collider2D playerHit = hits.FirstOrDefault(collider => (targetMask.value & (1 << collider.gameObject.layer)) != 0);
        if (playerHit != null)
        {
            if (playerHit.GetComponent<Health>().isInvulnerable) return HitType.Invulnerable;
            return HitType.Hit;
        }
        return HitType.Miss;
    }
}
