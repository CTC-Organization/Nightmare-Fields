using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CurvedProjectile : MonoBehaviour
{

    [Header("Curved Projectile Stats")]
    [SerializeField] private float travelTime = 40f;
    [SerializeField] private float arcHeight = 180f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject explosionVFX; // Efeito visual da explosão
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2d;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool hasReachedTarget = false;

    private float timer;
    private bool hasExploded = false;


    private void Start()
    {

        // Desativa a collider no início
        collider2d.enabled = false;

        // Guarda a posição inicial
        startPosition = transform.position;
    }

    public void Shoot(Vector2 direction, Vector2 targetPos)
    {
        timer = 0;
        targetPosition = targetPos;

        // Ajusta a direção para inclinar a trajetória
        direction = direction.normalized;

        // Define a velocidade inicial

        rb.linearVelocity = direction * (Vector2.Distance(startPosition, targetPosition) / travelTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Simula a trajetória inclinada (arco)
        SimulateArc();

        // Verifica se o projétil chegou perto do alvo
        if (!hasReachedTarget && Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            ReachedTarget();
        }

        if (!hasExploded && (timer >= travelTime || Vector2.Distance(transform.position, targetPosition) < 0.1f))
        {
            Explode();
        }
    }

    private void SimulateArc()
    {
        // Calcula o progresso da trajetória (0 a 1)
        float progress = Vector2.Distance(startPosition, transform.position) / Vector2.Distance(startPosition, targetPosition);

        float arc = arcHeight * Mathf.Sin(progress * Mathf.PI);
        transform.position = Vector2.Lerp(startPosition, targetPosition, progress) + Vector2.up * arc;
    }

    private void ReachedTarget()
    {
        hasReachedTarget = true;

        // Ativa a collider ao chegar no alvo
        collider2d.enabled = true;

        // Para o movimento do projétil
        rb.linearVelocity = Vector2.zero;

        // Destroi o projétil após um pequeno delay (opcional)
        Invoke("Explode", 0.25f);
    }
    private void Explode()
    {
        hasExploded = true;

        // Ativa a collider ao explodir
        collider2d.enabled = true;

        // Instancia o efeito visual da explosão
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        // Aplica dano aos inimigos próximos
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Health playerHealth = hitCollider.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeEnemyAttackDamage(HitType.Hit, 1); // Ajuste o dano conforme necessário
                }
            }
        }

        // Destroi o projétil
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se colidir com algo antes de explodir, explode imediatamente
        if (!hasExploded && collision.CompareTag("Player"))
        {
            Explode();
        }
    }
}