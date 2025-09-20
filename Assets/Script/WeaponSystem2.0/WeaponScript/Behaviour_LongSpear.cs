using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "WeaponBehaviour/Long Spear")]
public class Behaviour_LongSpear: WeaponBehaviour
{
    public float maxRange = 8.5f;
    public float hitBoxWidth = 3.5f;
    public float knockbackRange = 1.5f;
    public float knockbackDuration = 1.15f;
    public float stunDuration = 1f;
    public Vector3 lightEffectDefPos = new Vector3(0f, 0f, 0f);

    public GameObject projectilePrefab;
    public LayerMask affectedMask;

    public AudioClip[] inAirSFX;
    public AudioClip[] impactSFX;


    public override void LightAttack(int dmg, bool isCrit, float critMultiplier, Vector2 originalPosition, Vector2 mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats)
    {
        Vector2 mouseDirection = (mousePosition - originalPosition).normalized;
        Vector2 boxCenter = originalPosition + mouseDirection * (maxRange /2f);
        Vector2 boxSize = new Vector2(maxRange, hitBoxWidth);
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        Collider2D[] targets = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, affectedMask);
        if (targets.Length == 0)
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_light_NoHit, playerStats.transform, 1f);
            return;
        }

        else
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_light_Hit, playerStats.transform, 1f);
        }
        foreach (var  target in targets)
        {
            var enemyStats = target.GetComponent<AIStats>();
            var enemyBehaviour = target.GetComponent<AIBehaviour>();
            if (enemyStats != null)
            {
                int finalDmg = dmg;

                if (isCrit)
                {
                    finalDmg = Mathf.CeilToInt(finalDmg * critMultiplier);
                }


                //Debug.Log("Deal " + dmg +" to"+ target.name);
                playerStats.DealDmg(enemyStats, finalDmg, isCrit);
                enemyBehaviour.TakeKnockback(playerStats.transform, knockbackRange * 1.5f, knockbackDuration * 1.5f, stunDuration);




            }
            else
            {
                //Debug.Log("enemy is null");
            }
        }
    }

    public override void HeavyAttack(int dmg, bool isCrit, float critMultiplier, Vector2 originalPosition, Vector2 mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats)
    {
        Vector2 toMouse = (mousePosition - originalPosition).normalized;
        int finalDmg = dmg;
        if (isCrit)
        {
            finalDmg = Mathf.CeilToInt(finalDmg * critMultiplier);
        }

        GameObject projectileObj = Instantiate(projectilePrefab, originalPosition, Quaternion.identity);
        weaponHolder.DiscardWeapon();
       
        
        BulletBehaviour _bulletBehaviour = projectileObj.GetComponent<BulletBehaviour>();
        if (_bulletBehaviour != null)
        {
            _bulletBehaviour.arrowInAirSounds = inAirSFX; 
            _bulletBehaviour.arrowHitSounds = impactSFX;
            _bulletBehaviour.isDropAtImpact = true;
            //_bulletBehaviour.droppedGameObjPrefab = weaponHolder.droppedGOPrefab;
            _bulletBehaviour.layerMask = affectedMask;
            _bulletBehaviour.direction = toMouse;
            _bulletBehaviour._damage = finalDmg;
            _bulletBehaviour._isCrit = isCrit;
            float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
            projectileObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
    }

    public override IEnumerator ModifyMovement1(float distance, float duration, Vector2 direction, Transform user, PlayerInputControl playerInputControl)
    {
        Debug.Log("DoExtraMovement1 called");
        float elapsed = 0f;
        Vector2 start = user.position;
        Vector2 target = start + direction.normalized * distance;

        while (elapsed < duration)
        {
            Debug.Log("user.position before is: " + user.position);
            float t = elapsed / duration;
            user.position = Vector3.Lerp(start, target, t);

            elapsed += Time.deltaTime;
            yield return null; // <-- pause until next frame
        }
        Debug.Log("user.position after is: " + user.position);
    }

    public override IEnumerator ModifyMovement2(float distance, float duration, Vector2 direction, Transform user, PlayerInputControl playerInputControl)
    {
        yield return null;
    }

    public override IEnumerator PlayLightAttackAnimation1(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        Vector2 toMouse = (mousePosition - originalPosition);
        float mouseAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        Debug.Log("Mouse Angle is: " + mouseAngle);

        weaponTransform.rotation = Quaternion.Euler(0f, 0f, mouseAngle - 180f);

        weaponEffect.rotation = Quaternion.Euler(0f, 0f, mouseAngle);

        Vector3 scale = new Vector3(0.85f, 1f, 1f);
        weaponTransform.localScale = scale;
        yield return new WaitForSeconds(0.1f);
        scale = new Vector3(1.15f, 1f, 1f);
        weaponTransform.localScale = scale;
        Debug.Log("Euler rotation is: " + weaponTransform.eulerAngles);


        

        yield return new WaitForSeconds(0.15f);
        scale = new Vector3(1f, 1f, 1f);
        weaponTransform.localScale = scale;



    }
    public override IEnumerator PlayLightAttackAnimation2(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        Vector2 toMouse = (mousePosition - originalPosition);
        float mouseAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        Debug.Log("Mouse Angle is: " + mouseAngle);

        weaponTransform.rotation = Quaternion.Euler(0f, 0f, mouseAngle - 180f);

        weaponEffect.rotation = Quaternion.Euler(0f, 0f, mouseAngle);

        Vector3 scale = new Vector3(0.85f, 1f, 1f);
        weaponTransform.localScale = scale;
        yield return new WaitForSeconds(0.15f);
        scale = new Vector3(1.15f, 1f, 1f);
        weaponTransform.localScale = scale;
        Debug.Log("Euler rotation is: " + weaponTransform.eulerAngles);


       

        yield return new WaitForSeconds(0.25f);
        scale = new Vector3(1f, 1f, 1f);
        weaponTransform.localScale = scale;
    }

    public override IEnumerator PlayHeavyAttackAnimation(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        yield return null;
       
    }
}
