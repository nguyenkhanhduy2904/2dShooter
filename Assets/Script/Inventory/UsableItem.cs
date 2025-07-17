using UnityEngine;

[CreateAssetMenu(fileName = "UsableItem", menuName = "Scriptable Objects/UsableItem")]
public abstract class UsableItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public AudioClip[] activeSounds;
    public abstract void Use(GameObject user);
}
