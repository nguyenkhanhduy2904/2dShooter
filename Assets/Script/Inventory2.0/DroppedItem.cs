using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemData itemData;

    
    public void Initialize(ItemData data)
    {
        itemData = data;
        GetComponentInChildren<SpriteRenderer>().sprite = data.itemSprite;
    }
}
