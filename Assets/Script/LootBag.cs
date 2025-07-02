using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    List<Loot> getDroppedItems()
    {
        int random = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (random <= item.dropChance) 
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Debug.Log(possibleItems.Count + " dropped items");
            return possibleItems;
        }
        else
        {
            Debug.Log(possibleItems.Count + " dropped items");
            return null;
        }
    }

    public List<GameObject> InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> droppedItems = getDroppedItems();
        List<GameObject> droppedGameObjects = new List<GameObject>();
        if (droppedItems.Count > 0)
        {
            for (int i = 0; i < droppedItems.Count; i++)
            {
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponentInChildren<SpriteRenderer>().sprite = droppedItems[i].lootSprite;
                lootGameObject.GetComponent<PickUpLoot>().loot = droppedItems[i];
                droppedGameObjects.Add(lootGameObject);

                float dropForce = 500f;
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
            }
        }
        return droppedGameObjects;
    }
}
