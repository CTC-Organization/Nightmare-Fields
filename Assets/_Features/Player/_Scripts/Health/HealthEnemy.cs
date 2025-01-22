using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] private float startingEnemyHealth;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingEnemyHealth;
    }

    private void takeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingEnemyHealth);
        Debug.Log($"Enemy took damage: {_damage}, Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            // Enemy dies
            Debug.Log("Enemy Died");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
       
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingEnemyHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Bullet"))
        {
            Debug.Log("Enemy hit by Player_Bullet: " + collision.name);
            takeDamage(1);
        }
    }
}