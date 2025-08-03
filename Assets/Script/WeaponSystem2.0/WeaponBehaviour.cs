using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponBehaviour", menuName = "Scriptable Objects/WeaponBehaviour")]
public abstract class WeaponBehaviour : ScriptableObject
{
    public AudioClip[] weaponSounds;
    public GameObject weaponParticlePrefab;

    public abstract IEnumerator LightAttack(int dmg, bool isCrit, PlayerController _player);
    public abstract IEnumerator HeavyAttack(int dmg, bool isCrit, PlayerController _player);

}
