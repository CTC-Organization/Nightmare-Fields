using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector3 hitBoxSize;
    [SerializeField] private float attackStartDistance;
    [SerializeField] private Animator animator;

    [SerializeField] private Vector3 attackOrigin; // Dist�ncia de ataque
 

    private float lastAttackTime;
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

        /// <summary>
    /// Realiza o ataque do inimigo ao atingir o alvo e controla o cooldown entre ataques
    /// </summary>
    /// <returns>Retorna uma Coroutine que aguarda o cooldown do ataque</returns>
    IEnumerator Attack()
    {
        // Calcula a direção do alvo em relação ao inimigo
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        // Define a origem do ataque com base na direção calculada
        attackOrigin = transform.position + directionToTarget * attackStartDistance;


        //attackOrigin = transform.position + transform.up * attackStartDistance;
        animator.SetTrigger("Attack");

        LayerMask targetMask = 1 << target.layer;
        HitType hitType = HitBoxCreate(targetMask, attackOrigin);

        // Espera o tempo do cooldown antes de permitir um novo ataque
        lastAttackTime = Time.time;

        if (hitType == HitType.Hit)
        {
            target.GetComponent<Health>().TakeEnemyAttackDamage( hitType, damage);
            float targetHealth = target.GetComponent<Health>().currentHealth;
            Debug.Log($"Ataque realizado! {damage} de dano causado.\n HP do player = {targetHealth}");
        }
        // Ap�s o ataque, faz a IA voltar � sua trajet�ria
        yield return new WaitForSeconds(attackCooldown); // Aguarda o tempo do cooldown
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin, hitBoxSize);
    }

    HitType HitBoxCreate(LayerMask targetMask, Vector3 attackOrigin) // ataque "Realista" pode errar ou acertar
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackOrigin, new Vector2(hitBoxSize.x, hitBoxSize.y), 0);
        Collider2D hit = hits.FirstOrDefault(collider => (targetMask.value & (1 << collider.gameObject.layer)) != 0);
        if (hit != null) return HitType.Hit; // Substitua por seu tipo de retorno adequado
        return HitType.Miss; // Substitua por seu tipo de retorno adequado
    }
}
