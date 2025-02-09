using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.CompareTag("Player")) return;
            Debug.Log("Colis�o detectada com: " + collision.name);
            Health healthComponent = collision.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.AddHealth(healthValue);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Componente Health n�o encontrado no objeto: " + collision.name);
            }
        }
    }
}