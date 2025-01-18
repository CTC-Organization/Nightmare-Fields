using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log("Colisão detectada com: " + collision.name);
            Health healthComponent = collision.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.AddHealth(healthValue);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Componente Health não encontrado no objeto: " + collision.name);
            }
        }
        else
        {
            Debug.LogError("Colisão detectada, mas o objeto é null.");
        }
    }
}