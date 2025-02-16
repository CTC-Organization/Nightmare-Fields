using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtém o componente SpriteRenderer do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Atualiza o Sorting Order com base na posição Y
        // Quanto maior a posição Y, menor o Sorting Order
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}