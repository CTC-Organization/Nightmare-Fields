using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingEnemyHealth;
    private bool isDead; // blend tree buga animation event porque o float não é exatamente 1 ou 0 (logo as duas animações de mortes são executadas por debaixo dos panos - morrendo duas vezes),

    private float currentHealth;
    //private static int collisionCount = 0; // Contador de colisões

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
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void Die()
    {
        if (isDead || this == null) return;
        Debug.Log("Inimigo " + name + " morreu");
        GameManager.instance.EnemyCount();
        isDead = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         Debug.Log("trigger");
        if (collision.CompareTag("Player_Bullet"))
        {
            Debug.Log($"Enemy hit by Player_Bullet: {collision.name}");
            Destroy(collision.gameObject);
            TakeDamage(1);

            //collisionCount++; // Incrementa a contagem de colisões
            //Debug.Log($"Total de colisões: {collisionCount}");

            //GameManager.instance.CheckCollisionCount(collisionCount); // Verifica no GameManager
        }
        else if (collision.CompareTag("Player_Especial_Bullet"))
        {
            Debug.Log($"Enemy hit by Player_EspecialBullet: {collision.name}");
            Destroy(collision.gameObject);
            TakeDamage(2);
        }
    }
}
