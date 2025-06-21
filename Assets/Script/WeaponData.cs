using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public GameObject bulletPrefab;
    public AudioClip[] shootSounds;
    public AudioClip[] clipEmptySound;
    public AudioClip[] reloadProcedure;

    public float fireRate = 5f;
    public int damage = 10;
    public float bulletSpeed = 20f;
    public bool isAutomatic = false;
    public GameObject weaponPrefab; // assign prefab here
    public int magSize;
}
