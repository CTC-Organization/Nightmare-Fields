using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool isInvulnerable = false;

    private void Awake()
    {
        currentHealth = startingHealth;
        Debug.Log($"Initial Health: {currentHealth}");
    }

    private void takeDamage(float _damage)
    {
        if (!isInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            Debug.Log($"Took Damage: {_damage}, Current Health: {currentHealth}");

            if (currentHealth > 0)
            {
                //hurt
                Debug.Log("Player Hurt");
                StartCoroutine(InvulnerabilityCoroutine());
            }
            else
            {
                //die
                Debug.Log("Player Died");
            }
        }
        else
        {
            Debug.Log("Player is invulnerable and did not take damage.");
        }
    }

    private void Update()
    {
        //losing life
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key 'E' Pressed");
            takeDamage(1);
        }
        //adding life
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Key 'P' Pressed");
            AddHealth(1);
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        Debug.Log($"Healed: {_value}, Current Health: {currentHealth}");
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        Debug.Log("Player is now invulnerable.");
        yield return new WaitForSeconds(2f);
        isInvulnerable = false;
        Debug.Log("Player is no longer invulnerable.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy: " + collision.name);
            takeDamage(1);
        }
    }
}