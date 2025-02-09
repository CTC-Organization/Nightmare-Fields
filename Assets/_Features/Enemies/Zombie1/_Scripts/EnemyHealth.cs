using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingEnemyHealth;
    private float currentHealth;
    private static int collisionCount = 0; // Contador de colisões

    private void Start()
    {
        currentHealth = startingEnemyHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingEnemyHealth);
        Debug.Log($"Dano no inimigo ({name}) recebido: {damage}, \nvida atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            SetDying();
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingEnemyHealth);
    }

    private void SetDying()
    {
        Debug.Log("Inimigo esta morrendo");
        EnemyMovement em = GetComponent<EnemyMovement>();
        em.isDying = true;
        em.animator.SetBool("IsDying", em.isDying);
    }

    public void Die()
    {
        Debug.Log("Inimigo " + name + " morreu");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Bullet"))
        {
            Debug.Log($"Enemy hit by Player_Bullet: {collision.name}");
            Destroy(collision.gameObject);
            TakeDamage(1);

            collisionCount++; // Incrementa a contagem de colisões
            Debug.Log($"Total de colisões: {collisionCount}");

            GameManager.instance.CheckCollisionCount(collisionCount); // Verifica no GameManager
        }
    }
}
