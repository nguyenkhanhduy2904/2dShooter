using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public GameObject droppedItemPrefab;
    public InventoryDisplay display;
    [SerializeField]public List<InventorySlot> tempHolder;

    bool isPlayer = false;
    PlayerController _player;

    public void Add(ItemData item, int quantity = 1)
    {
        var slot = inventorySlots.Find(s => s.itemData == item);
        if (slot != null)
        {
            slot.quantity += quantity;
        }
        else
        {
            inventorySlots.Add(new InventorySlot(item, quantity));
        }
        if(isPlayer)
        display.RefreshInventoryUI();
    }

    public void Remove(ItemData item, int quantity = 1)
    {
        var slot = inventorySlots.Find(s => s.itemData == item);
        if (slot != null)
        {
            slot.quantity -= quantity;
            if (slot.quantity <= 0) inventorySlots.Remove(slot);
        }
        if (isPlayer)
            display.RefreshInventoryUI();
    }


    public List<GameObject> InstantiateItem(Vector3 spawnPosition)
    {
        List<GameObject> droppedGameObjects = new List<GameObject>();
        foreach (var slot in inventorySlots)
        {
            for (int i = 0; i < slot.quantity; i++)
            {
                GameObject itemObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                DroppedItem dropped = itemObject.GetComponent<DroppedItem>();
                dropped.Initialize(slot.itemData);
                droppedGameObjects.Add(itemObject);
            }
        }
        return droppedGameObjects;
    }



    private void Start()
    {
        _player = GetComponent<PlayerController>();
        foreach (var item in tempHolder)
        {
            inventorySlots.Add(item);
        }
        if ( _player != null) 
        { 
            isPlayer= true;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CompareTag("Player"))
        {
            DroppedItem droppedItem = collision.GetComponent<DroppedItem>();
            if (droppedItem != null) 
            {
                Add(droppedItem.itemData);
                Destroy(collision.gameObject);
                // Debug print current inventory
                foreach (var slot in inventorySlots)
                {
                    Debug.Log($"{slot.itemData.itemName} x{slot.quantity}");
                }
            }
        }
    }


    private void Update()
    {
        if (!isPlayer || !_player.isAlive ) return;
        for (int i = 0; i< Mathf.Min( inventorySlots.Count, 9); i++) 
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TryUseItemAtIndex(i);
            }
           
        }
    }

    public void TryUseItemAtIndex(int index)
    {
        if (index < 0 || index >= inventorySlots.Count) return;
        //if (inventorySlots[index].itemData.itemType != ItemType.UntargetActive || inventorySlots[index].itemData.itemType != ItemType.TargetActive) return;
        inventorySlots[index].itemData.itemEffect.ApplyEffect(this.gameObject);
        Remove(inventorySlots[index].itemData);
    }

}
