using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject    
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public UsableItem usableItemRef;
    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
