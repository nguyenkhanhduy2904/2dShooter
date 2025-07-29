using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public ItemType itemType;
    public ItemEffect itemEffect; 
}


public enum ItemType{
    UntargetActive,
    TargetActive,
    NonActive,
    Currency,
    Ammo
}