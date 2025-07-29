using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/FreezeEffect")]
public class FreezeEffect : ItemEffect
{
    public SceneManager SceneManager;
    public GameObject AoEEffect;
    public Vector3 scale = Vector3.one;
    public override void ApplyEffect(GameObject user)
    {
        var _player = user.GetComponent<PlayerController>();
        SceneManager.SpawnAOEEffect(user.transform.position, _player.weaponHolder.RotateToMouse(), AoEEffect, scale);
    }
}
