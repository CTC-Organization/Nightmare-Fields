using TopDown.Shooting;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedValue = 5;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;
    public GunController gunController;

    int selectedSlot = -1;

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

        if (Input.GetKeyDown(KeyCode.V)) // Suponha que "E" seja a tecla de uso
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
        Item selectedItem = GetSelectedItem(remove: true); // Remove o item após o uso

        if (selectedItem is PowerUp powerUp)
        {
            // Ativa o PowerUp se o item for do tipo PowerUp
            powerUp.ActivatePowerUp(gunController);
        }
        else
        {
            Debug.LogWarning("Item selecionado não é um PowerUp!");
        }
    }





    private Item GetSelectedItem(bool remove = false)
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
                    Destroy(inventoryItem.gameObject); // Remove o item do inventário após o uso
                }
                return item;
            }
        }

        return null;
    }

}
