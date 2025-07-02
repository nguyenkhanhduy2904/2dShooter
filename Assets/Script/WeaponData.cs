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
    public AudioClip[] pickUpSounds;
    public AudioClip[] dropSounds;

    public float fireRate = 5f;
    public int damage = 10;
    public float bulletSpeed = 20f;
    public bool isAutomatic = false;
    public GameObject weaponPrefab; // assign prefab here
    public int magSize;
    public int reserveAmmo;
    public float spreadAngleMax;
    public float spreadAngleMin;
    public float spreadAngleIncreasePerShot;
    public float spreadRecoveryRate;

    public int weaponCritChance;
    public float weaponCritMultiplier;

    public int pierceAmount = 1;

    public GameObject pickupPrefab; // assign in the Inspector
}
