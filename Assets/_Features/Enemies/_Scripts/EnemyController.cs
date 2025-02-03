using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    //[Header("Configura��es de Movimento")]
    //[Tooltip("A velocidade do movimento do inimigo")]
    //[SerializeField] private float speed = 2.5f; // Velocidade de movimento

    [Header("Configura��es de Ataque")]

    [Tooltip("O dano causado pelo inimigo ao atingir o alvo")]
    [SerializeField] private int damage = 10; // Quantidade de dano


    [Tooltip("O tempo de espera entre os ataques")]
    [SerializeField] private float attackCooldown = 1f; // Tempo entre ataques
    private Coroutine attackCoroutine;

    [Tooltip("�rea de colis�o de para o Attack")]
    [SerializeField] private Vector3 hitBoxSize; // Dist�ncia de ataque

    [SerializeField] private Vector3 attackOrigin; // Dist�ncia de ataque
    [SerializeField] private float attackStartDistance;

    private float lastAttackTime = 0f; // Tempo do �ltimo ataque

    [Header("Configura��es de VFX")]
    [SerializeField] Animator animator;

    [Header("Refer�ncias")]

    [Tooltip("Refer�ncia ao componente AIDestinationSetter")]
    [SerializeField] private AIDestinationSetter aiDestinationSetter; // Refer�ncia ao AIDestinationSetter para controlar o destino

    [Tooltip("Objeto alvo do inimigo (por exemplo, o jogador)")]
    [SerializeField] private GameObject target = null; // Alvo da IA (por exemplo, o jogador)

    private IAstarAI ai; // Refer�ncia ao componente IAstarAI

    void Start()
    {

        target = GameObject.FindWithTag("Player");
        if (aiDestinationSetter == null)
            aiDestinationSetter = GetComponent<AIDestinationSetter>();

        ai = GetComponent<IAstarAI>(); // Obt�m o componente IAstarAI

        SetTarget(target.transform);

    }

    void Update()
    {

        // Verifica se a IA atingiu o destino (dentro do alcance de endReachedDistance)
        if (ai.reachedDestination && Time.time - lastAttackTime >= attackCooldown)
        {
            // Realiza o ataque
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            attackCoroutine = StartCoroutine(Attack(target));
        }
    }

    /// <summary>
    /// Atualiza o alvo do inimigo
    /// </summary>
    /// <param name="newTarget">O novo alvo para o inimigo seguir</param>
    public void SetTarget(Transform newTarget)
    {
        if (aiDestinationSetter != null)
            aiDestinationSetter.target = newTarget;

    }

    /// <summary>
    /// Realiza o ataque do inimigo ao atingir o alvo e controla o cooldown entre ataques
    /// </summary>
    /// <returns>Retorna uma Coroutine que aguarda o cooldown do ataque</returns>
    IEnumerator Attack(GameObject targetToAttack)
    {
        attackOrigin = transform.position + transform.up * attackStartDistance;
        animator.SetTrigger("Attack");

        LayerMask targetMask = 1 << targetToAttack.layer;
        HitType hitType = HitBoxCreate(targetMask, attackOrigin);

        // Espera o tempo do cooldown antes de permitir um novo ataque
        lastAttackTime = Time.time;

        if (hitType == HitType.Hit)
        {
            targetToAttack.GetComponent<PlayerMovement>().TakeDamage(hitType, damage);
            float targetHealth = targetToAttack.GetComponent<PlayerMovement>().currentHealth;
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
