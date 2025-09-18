using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData2", menuName = "Scriptable Objects/WeaponData2")]
public class WeaponData2 : ScriptableObject
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }
    [Header("Stat")]
    public string weaponName;
    public Sprite weaponSprite;
    public WeaponType weaponType;
    public int weaponBaseDmg;
    public float weaponSpeed;
    [Range(1, 100)]
    public int critChance;
    public float critMultiplier;

    [Header("Visual Transform Settings")]
    //public Vector3 spriteLocalPositionRight = Vector3.zero;
    //public Vector3 spriteLocalPositionLeft = Vector3.zero;

    public Vector3 spriteLocalRotationRight = Vector3.zero; // Euler angles
    public Vector3 spriteLocalRotationLeft = Vector3.zero; // Euler angles
    public Vector3 spriteLocalScale = Vector3.one;
    public Sprite sliceEffect_light;
    public Sprite sliceEffect_heavy;
    [Header("Swing Settings")]
    public Vector3 weaponPivotOffset = Vector3.zero; // Where weapon rotates from

    [Header("Component")]
    public WeaponBehaviour weaponBehaviour;

}
