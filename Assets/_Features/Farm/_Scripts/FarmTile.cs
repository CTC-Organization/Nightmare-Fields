using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public enum PlantState { None, Seed, Sprout, Grown }

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
        if (currentPlantState == PlantState.None)
        {
            PlantSeed();
        }
        else if (currentPlantState != PlantState.Grown)
        {
            isWatered = true;
        }
        else
        {
            HarvestPlant();
        }
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
        Destroy(currentPlant);
        int nextStage = (int)currentPlantState + 1;
        if (nextStage < plantPrefabs.Length)
        {
            currentPlant = Instantiate(plantPrefabs[nextStage], transform.position, Quaternion.identity, transform);
            currentPlantState = (PlantState)nextStage;
        }
    }

    private void HarvestPlant()
    {
        Destroy(currentPlant);
        currentPlantState = PlantState.None;
    }
}
