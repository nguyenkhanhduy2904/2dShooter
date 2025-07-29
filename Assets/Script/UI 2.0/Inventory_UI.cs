using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory; // Link your Inventory script in the Inspector
    public Transform inventoryPanel; // The UI parent (panel with vertical layout, etc.)
    public GameObject slotPrefab; // A prefab with TMP_Text components

    private List<GameObject> displayedSlots = new List<GameObject>();

    private void Start()
    {
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        // Clear old entries
        foreach (GameObject go in displayedSlots)
        {
            Destroy(go);
        }
        displayedSlots.Clear();

        // Create UI elements for each inventory slot
        foreach (var slot in inventory.inventorySlots)
        {
            GameObject slotUI = Instantiate(slotPrefab, inventoryPanel);
            InvenSlot invenSlot = slotUI.GetComponent<InvenSlot>();

            if (invenSlot != null && slot.itemData != null)
            {
                invenSlot.SetSlot(slot.itemData.itemSprite, slot.quantity);
            }

            displayedSlots.Add(slotUI);
        }
    }

    private void Update()
    {
        //RefreshInventoryUI();
        
    }

}
