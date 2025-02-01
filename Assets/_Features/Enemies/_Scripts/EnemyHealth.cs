using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingEnemyHealth;
    private float currentHealth;

    private void Start()
    {
        currentHealth = startingEnemyHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingEnemyHealth);
        Debug.Log($"Dano no inimigo ({name}) recebido: {damage}, \nvita atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingEnemyHealth);
    }

    private void Die()
    {
        Debug.Log("Inimigo " + name + " morreu");
        Destroy(gameObject); // colocar animação - o final da animação ativa a função de destuir o objeto em destruir
        // essa função deve appenas ativar um boolean isDead e ativar o animator para ativara a animação de morte (no final dispara o evento de destruir)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Bullet"))
        {
            Debug.Log($"Enemy hit by Player_Bullet: {collision.name}");
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }
}
