using System.Collections.Generic;
using TopDown.Shooting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxStackedValue = 5;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;
    public GunController gunController;

    int selectedSlot = -1;

    private Dictionary<int, Item> savedInventory = new Dictionary<int, Item>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o inventário entre cenas
        }
        else
        {
            Destroy(gameObject); // Garante que só exista um inventário
        }
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        gunController = GameObject.FindWithTag("Player")?.GetComponent<GunController>();

        if (gunController == null)
        {
            Debug.LogError("GunController não foi encontrado. Verifique se o Player tem a tag 'Player' e o componente GunController.");
        }
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            UseSelectedItem();
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            InventorySlots[selectedSlot].Deselect();
        }

        InventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public void UseSelectedItem()
    {
        Item selectedItem = GetSelectedItem(remove: false);

        if (selectedItem is PowerUp powerUp)
        {
            powerUp.ActivatePowerUp(gunController);
            selectedItem = GetSelectedItem(remove: true);
        }
        else
        {
            Debug.LogWarning("Item selecionado não é um PowerUp!");
        }
    }

    public Item GetSelectedItem(bool remove = false)
    {
        if (selectedSlot < 0 || selectedSlot >= InventorySlots.Length)
            return null;

        InventorySlot slot = InventorySlots[selectedSlot];

        if (slot.transform.childCount > 0)
        {
            InventoryItem inventoryItem = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            if (inventoryItem != null)
            {
                Item item = inventoryItem.item;
                if (remove)
                {
                    Destroy(inventoryItem.gameObject);
                }
                return item;
            }
        }

        return null;
    }

    public void AddPowerUpToInventory()
    {
        foreach (var slot in InventorySlots)
        {
            if (slot.transform.childCount == 0)  // Se o slot estiver vazio
            {
                // Instancia o prefab do PowerUp 1 no slot
                GameObject newItem = Instantiate(Resources.Load("PowerUp 1"), slot.transform) as GameObject;
                if (newItem == null)
                {
                    Debug.LogError("Prefab PowerUp 1 não encontrado na pasta Resources.");
                    return;
                }

                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();

                // Se o prefab PowerUp estiver configurado corretamente, basta adicionar
                PowerUp powerUp = newItem.GetComponent<PowerUp>();
                if (powerUp != null)
                {
                    inventoryItem.InitializeItem(powerUp); // Configura o item no slot de inventário
                }

                Debug.Log("PowerUp adicionado ao inventário!");
                return;
            }
        }

        Debug.LogWarning("Inventário cheio! Não foi possível adicionar o PowerUp.");
    }

    public void SaveInventory()
    {
        savedInventory.Clear();
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            Item item = GetItemFromSlot(i);
            if (item != null)
            {
                savedInventory[i] = item;
            }
        }
    }

    public void LoadInventory()
    {
        foreach (var kvp in savedInventory)
        {
            AddItemToSlot(kvp.Value, kvp.Key);
        }
    }

    private Item GetItemFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= InventorySlots.Length)
            return null;

        InventorySlot slot = InventorySlots[slotIndex];

        if (slot.transform.childCount > 0)
        {
            InventoryItem inventoryItem = slot.transform.GetChild(0).GetComponent<InventoryItem>();
            return inventoryItem?.item;
        }
        return null;
    }

    private void AddItemToSlot(Item item, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= InventorySlots.Length)
            return;

        InventorySlot slot = InventorySlots[slotIndex];

        if (slot.transform.childCount == 0) // Só adiciona se o slot estiver vazio
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(item);
        }
    }


}
