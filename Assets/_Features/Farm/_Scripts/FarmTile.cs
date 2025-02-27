using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FarmTile : MonoBehaviour
{
    public enum PlantState { None, Seed, Grown }

    public PlantState currentPlantState = PlantState.None;
    public GameObject[] plantPrefabs;
    private GameObject currentPlant;
    private bool isWatered = false;
    private int lastGrowthDay;
    private PlayerMovement player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        lastGrowthDay = DayManager.dm.days;

        // Recuperar estado salvo e posição
        (PlantState savedState, Vector3 savedPosition) = GameManager.instance.GetFarmTileState(transform.position);
Debug.Log($"Estado recuperado: {savedState} - {savedPosition}");


        if (savedState != PlantState.None)
        {
            currentPlantState = savedState;
            currentPlant = Instantiate(plantPrefabs[1], savedPosition, Quaternion.identity, transform);
        }
    }




    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PlayerIsNear())
        {
            Interact();
        }

        if (currentPlantState != PlantState.None && DayManager.dm.days > lastGrowthDay && isWatered)
        {
            GrowPlant();
            lastGrowthDay = DayManager.dm.days;
            isWatered = false;
        }
    }

    private bool PlayerIsNear()
    {
        return Vector2.Distance(transform.position, player.transform.position) < 1.5f;
    }

    private void Interact()
    {
        Debug.Log(currentPlantState);

        if (currentPlantState == PlantState.None)
        {
            if (CanPlantSeed()) // ✅ Verifica se tem semente no inventário
            {
                PlantSeed();
            }
            else
            {
                Debug.LogWarning("Você precisa de uma semente para plantar!");
                return;
            }
        }
        else if (currentPlantState == PlantState.Seed && isWatered == false)
        {
            if (CanWater())
            {
                isWatered = true;
            }
        }
        else if (currentPlantState == PlantState.Grown)
        {
            HarvestPlant();
        }

        GameManager.instance.SaveFarmTileState(transform.position, currentPlantState, currentPlant.transform.position);
    }

    // ✅ Função para verificar se o jogador tem sementes
    private bool CanPlantSeed()
    {
        Item selectedItem = InventoryManager.Instance.GetSelectedItem(remove: false);

        if (selectedItem != null && selectedItem.type.Equals(Item.ItemType.Seed))
        {
            selectedItem = InventoryManager.Instance.GetSelectedItem(remove: true);
            return true;
        }

        return false;
    }

    private bool CanWater()
    {
        Item selectedItem = InventoryManager.Instance.GetSelectedItem(remove: false);

        if (selectedItem != null && selectedItem.type.Equals(Item.ItemType.Oil))
        {
            selectedItem = InventoryManager.Instance.GetSelectedItem(remove: true);
            return true;
        }

        return false;
    }

    private void HarvestPlant()
    {
        Destroy(currentPlant);
        currentPlantState = PlantState.None;

        // ✅ Adiciona um PowerUp do tipo TripleShot ao inventário
        InventoryManager.Instance.AddPowerUpToInventory();
    }





    private void PlantSeed()
    {
        if (currentPlant == null)
        {
            currentPlant = Instantiate(plantPrefabs[0], transform.position, Quaternion.identity, transform);
            currentPlantState = PlantState.Seed;
        }
    }

    private void GrowPlant()
    {
        int nextStage = (int)currentPlantState + 1;
        if (nextStage < plantPrefabs.Length)
        {
            Destroy(currentPlant);
            currentPlant = Instantiate(plantPrefabs[nextStage], transform.position, Quaternion.identity, transform);

            // Move a nova planta para cima
            currentPlant.transform.position += Vector3.up;

            currentPlantState = PlantState.Grown;

            // Salvar estado e nova posição
            GameManager.instance.SaveFarmTileState(transform.position, currentPlantState, currentPlant.transform.position);
        }
    }
}
