using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponBehaviour", menuName = "Scriptable Objects/WeaponBehaviour")]
public abstract class WeaponBehaviour : ScriptableObject
{
    public AudioClip[] weaponSounds_light_NoHit;
    public AudioClip[] weaponSounds_light_Hit;

    public AudioClip[] weaponSounds_heavy_NoHit;
    public AudioClip[] weaponSounds_heavy_Hit;
    public GameObject weaponParticlePrefab;
    

    public float lightAttackCoolDown = 1f;
    public float heavyAttackCoolDown = 1.5f;
    public float comboTimeWindow = 1.5f;

    public float chargeDistance = 0.5f;
    public float chargeTime = 0.15f;

    public abstract IEnumerator ModifyMovement1(float distance, float duration, Vector2 direction, Transform user, PlayerInputControl playerInputControl);
    public abstract IEnumerator ModifyMovement2(float distance, float duration, Vector2 direction, Transform user, PlayerInputControl playerInputControl);
    //public abstract IEnumerator Knockback(Transform user, Transform target, float distance, float duration);
    public abstract void LightAttack(int dmg, bool isCrit, float critMultiplier, Vector2 originalPosition, Vector2  mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats);
    public abstract void HeavyAttack(int dmg, bool isCrit, float critMultiplier, Vector2 originalPosition, Vector2 mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats);

    public abstract IEnumerator PlayLightAttackAnimation1(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect);
    public abstract IEnumerator PlayLightAttackAnimation2(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect);

    public abstract IEnumerator PlayHeavyAttackAnimation(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect);

}
