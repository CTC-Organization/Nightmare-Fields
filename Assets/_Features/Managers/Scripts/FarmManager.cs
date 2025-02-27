using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;
    public GameObject plantPrefab; // Prefab da plantação

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
