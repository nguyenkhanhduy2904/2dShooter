using UnityEngine;
using System.Collections;
using System;

public class DroppedItem : MonoBehaviour
{
    public ItemData itemData;
    public Collider2D collider2D;
    
    public void Initialize(ItemData data)
    {
        Debug.Log("Initialize called");
        itemData = data;
        Debug.Log(itemData.name);
        GetComponentInChildren<SpriteRenderer>().sprite = data.itemSprite;
    }

    private void Start()
    {
        collider2D.enabled = false;
        StartCoroutine(StartCountDown(1f, ()=> collider2D.enabled = true));

    }

    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSecondsRealtime(duration);
        onFinished?.Invoke();
    }
}
