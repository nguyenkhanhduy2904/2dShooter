using UnityEngine;
using System.Collections;

public abstract class ItemEffect : ScriptableObject
{
    public AudioClip[] FXSound;
    public GameObject ParticalPrefab;
    public abstract void ApplyEffect(GameObject user);

   
    
}
