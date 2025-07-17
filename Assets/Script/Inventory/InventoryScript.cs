
using UnityEngine;
using System.Collections.Generic;

public class InventoryScript : MonoBehaviour
{
    public List<ItemSlot> slots = new List<ItemSlot>();
    private GameObject _player;

    public void AddItem(UsableItem item, int amount)
    {
        // Check if the item already exists
        ItemSlot existing = slots.Find(s => s.item == item);
        if (existing != null)
        {
            existing.quantity += amount;
        }
        else
        {
            slots.Add(new ItemSlot
            {
                item = item,
                quantity = amount
            });
        }
        FindFirstObjectByType<InventoryUI>().RefreshUI();
        Debug.Log($"Added {amount} x {item.itemName}");
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= slots.Count)
        {
            Debug.Log("No item in this slot.");
            return;
        }

        ItemSlot slot = slots[index];
        if (slot.quantity <= 0)
        {
            Debug.Log("No more of this item.");
            return;
        }

        slot.item.Use(_player);
        slot.quantity--;

        Debug.Log($"Used {slot.item.itemName}. Remaining: {slot.quantity}");

        // Optionally remove empty slots
        if (slot.quantity <= 0)
        {
            slots.RemoveAt(index);
        }
        FindFirstObjectByType<InventoryUI>().RefreshUI();
    }
    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>().gameObject;

    }
    public void Update()
    {
        for (int i = 0; i < Mathf.Min(slots.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
        }
    }

    

}
