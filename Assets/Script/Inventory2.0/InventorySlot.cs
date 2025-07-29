[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.itemData = item;
        this.quantity = quantity;
    }

    // You may want a parameterless constructor for Unity's serialization:
    public InventorySlot() { }
}
