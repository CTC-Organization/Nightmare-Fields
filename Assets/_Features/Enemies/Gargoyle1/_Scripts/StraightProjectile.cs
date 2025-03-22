using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StraightProjectile : MonoBehaviour
{
    [Header("Straight Projectile Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private Rigidbody2D rb;
    private float lifeTimer;

    public void Shoot(Vector2 direction)
    {
        lifeTimer = 0;
        rb.linearVelocity = direction * speed;
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
        if (collision.CompareTag("Player"))
        {
            // Causa dano ao jogador
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeEnemyAttackDamage(HitType.Hit, 1); // Ajuste o dano conforme necessário
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

}