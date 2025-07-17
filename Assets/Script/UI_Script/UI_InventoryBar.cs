using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public InventoryScript inventory;
    public GameObject slotPrefab;
    public Transform slotsParent;

    private List<UIInventorySlot> uiSlots = new List<UIInventorySlot>();

    private void Start()
    {
        // Create 9 slots
        for (int i = 0; i < 9; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            UIInventorySlot uiSlot = slotGO.GetComponent<UIInventorySlot>();
            uiSlots.Add(uiSlot);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < inventory.slots.Count)
            {
                uiSlots[i].SetSlot(inventory.slots[i]);
            }
            else
            {
                uiSlots[i].SetSlot(null);
            }
        }
    }
}
