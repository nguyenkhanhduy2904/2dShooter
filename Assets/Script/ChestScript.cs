using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    bool _wasOpened = false;
    [SerializeField] Sprite _chestOpenedSprite;
    SpriteRenderer _spriteRenderer;
    [SerializeField] AudioClip[] _chestOpenSounds;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_wasOpened) return;
       

        if (collision.CompareTag("Player"))
        {
            Debug.Log("player enter the chest radius");
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.sprite = _chestOpenedSprite;
            SoundFXManager.Instance.PlaySoundFXClip(_chestOpenSounds, transform, 1f);
            
            Vector3 offset = new Vector3 (Random.Range(-1f, 1), Random.Range(-1f, 1));
            //List<GameObject> droppedItems = GetComponent<LootBag>().InstantiateLoot(transform.position);
            List<GameObject> droppedItems = GetComponent<Inventory>().InstantiateItem(transform.position);

            _wasOpened = true;

            foreach (GameObject item in droppedItems) 
            {
                item.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(EnableColliderAfterDelay(item.GetComponent<Collider2D>(), 1f));
            }
        }
    }

    IEnumerator EnableColliderAfterDelay(Collider2D collision , float delay)
    {
        yield return new WaitForSeconds(delay);
        collision.enabled = true;
    }
        
    
    
}
