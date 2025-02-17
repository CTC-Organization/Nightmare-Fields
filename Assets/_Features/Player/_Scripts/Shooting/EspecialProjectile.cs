using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EspecialProjectile : MonoBehaviour
{
    [Header("Especial Movement Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float explosionRadius;
    private Rigidbody2D body;
    private float lifeTimer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void ShootEspecialBullet(Vector2 direction)
    {
        lifeTimer = 0;
        body.linearVelocity = Vector2.zero; // Corrigido de linearVelocity para velocity
        transform.up = direction; // Ajusta a rota��o do proj�til para a dire��o do tiro
        gameObject.SetActive(true);

        body.linearVelocity = direction * speed; // Define a velocidade constante do proj�til
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Aplica dano ao zumbi atingido
            ApplyDamage(collision.gameObject);

            // Aplica dano aos zumbis pr�ximos
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    ApplyDamage(hitCollider.gameObject);
                }
            }

            // Destroi o proj�til ap�s a colis�o
            Destroy(gameObject);
        }
    }

    private void ApplyDamage(GameObject enemy)
    {
        // Supondo que o inimigo tenha um componente de script chamado "EnemyHealth"
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha o raio da explos�o no editor para visualiza��o
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}