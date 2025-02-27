using UnityEngine;
using System.Collections.Generic;

public class DropController : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject prefab; // Prefab do drop
        public float weight;     // Peso do drop (probabilidade relativa)
    }

    public List<DropItem> dropTable = new List<DropItem>(); // Tabela de drops com pesos

    // Método para sortear um drop baseado nos pesos
    public GameObject GetRandomDrop()
    {
        // Calcula o peso total da tabela de drops
        float totalWeight = 0f;
        foreach (var drop in dropTable)
        {
            totalWeight += drop.weight;
        }

        // Sorteia um valor aleatório entre 0 e o peso total
        float randomValue = Random.Range(0, totalWeight);

        // Itera sobre a tabela de drops para encontrar o drop sorteado
        foreach (var drop in dropTable)
        {
            if (randomValue < drop.weight)
            {
                return drop.prefab; // Retorna o prefab sorteado
            }
            randomValue -= drop.weight; // Subtrai o peso do drop do valor aleatório
        }

        return null; // Caso não haja drops na tabela
    }

    // Método para instanciar o drop em uma posição específica
    public void SpawnDrop(Transform spawnPosition)
    {
        GameObject dropPrefab = GetRandomDrop(); // Sorteia um drop
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, spawnPosition.position, Quaternion.identity); // Instancia o drop
        }
        else
        {
            Debug.LogWarning("Nenhum drop foi sorteado ou a tabela de drops está vazia.");
        }
    }
}