using Assets.Script;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "WeaponBehaviour/Giant Axe")]
public class Behaviour_GiantAxe: WeaponBehaviour
{
    [SerializeField] public GameObject sliceEffectPrefab;
   
    [Range(0f, 360f)]
    public float swingArc_light = 180f;
    [Range(0f, 360f)]
    public float swingArc_heavy = 360f;
    public float maxRange = 5f;
    public LayerMask affectedMask;

    public float knockbackRange = 0.5f;
    public float knockbackDuration = 0.15f;
    public float stunDuration = 1f;

   

    public override void LightAttack(int dmg, bool isCrit , float critMultiplier, Vector2 originalPosition, Vector2 mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(originalPosition, maxRange, affectedMask);
        Vector2 swingDirection = (mousePosition - originalPosition).normalized;
        float halfSwingAngle = swingArc_light / 2;// should caching this for better performance, but the effect was negligible


        //draw debug lines(notes below)
                /* To create each edge, we need to rotate the "swing direction"
                * (the line from originalPosition to the mousePos ) [totalArc /2] degree of the arc to each side
                */

        Vector2 leftEdgeDirection = Quaternion.Euler(0, 0, -halfSwingAngle) * swingDirection;
        Vector2 rightEdgeDirection = Quaternion.Euler(0, 0, halfSwingAngle) * swingDirection;

                //line end at max range

        Vector2 leftEnd = originalPosition + leftEdgeDirection * maxRange;
        Vector2 rightEnd = originalPosition + rightEdgeDirection * maxRange;


        Debug.DrawLine(originalPosition, leftEnd, Color.red, 0.1f);
        Debug.DrawLine(originalPosition, mousePosition, Color.red, 0.1f);
        Debug.DrawLine(originalPosition, rightEnd, Color.red, 0.1f);

        if(targets.Length == 0)
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_light_NoHit, playerStats.transform, 1f);
            return;
        }

        else
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_light_Hit, playerStats.transform, 1f);
        }

            //core damage apply logic
            foreach (var target in targets)
            {
                //Debug.Log("there" + targets.Length + "object");
                Vector2 directionToTarget = (((Vector2)target.transform.position - originalPosition).normalized);
                float angleBetweenSwingToTarget = Vector2.Angle(swingDirection, directionToTarget);

                if (angleBetweenSwingToTarget <= halfSwingAngle)
                {
                    //Debug.Log("object was in attack hitbox");
                    var enemyStats = target.GetComponent<AIStats>();
                    var enemyBehaviour = target.GetComponent<AIBehaviour>();
                    if (enemyStats != null)
                    {
                        int finalDmg = dmg;
                        
                        if (isCrit)
                        {
                            finalDmg = Mathf.CeilToInt(finalDmg * critMultiplier);
                        }
                        
                        playerStats.DealDmg(enemyStats, finalDmg, isCrit);
                        enemyBehaviour.TakeKnockback(playerStats.transform, knockbackRange, knockbackDuration, stunDuration);
                        

                    

                    }

                    else
                    {
                        //Debug.Log("enemy is null");
                    }

                }
            }
        //yield return new WaitForSeconds(lightAttackCoolDown);


       
        
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
        Debug.Log("DoExtraMovement2 called");
        float elapsed = 0f;
        

        while (elapsed < duration)
        {
           
            playerInputControl.movementInputAllow =false;
            elapsed += Time.deltaTime;
            yield return null; // <-- pause until next frame
        }
        
        playerInputControl.movementInputAllow = true;
        Debug.Log("Finish ModifyMovement2");
    }

    public override IEnumerator PlayLightAttackAnimation1(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        // Step 1: Get the direction from the player to the mouse
        Vector2 toMouse = mousePosition - originalPosition;

        // Step 2: Convert that direction into an angle (in degrees)
        // This angle is measured from the right (0°), going counter-clockwise
        float mouseAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;

        // Step 3: Decide which way to rotate based on mouse position
        // If mouse is on the right, rotate 90° clockwise from it
        // If mouse is on the left, rotate 90° counter-clockwise from it
        float rotateDirection = toMouse.x >= 0 ? 90f : -90f;

        // Step 4: Add the offset and normalize the angle to stay within 0–360°
        float finalAngle = (mouseAngle + rotateDirection + 360f) % 360f;

        // Step 5: Instantly rotate the weapon to that angle
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
        weaponEffect.rotation = Quaternion.Euler(0f, 0f, mouseAngle);

        bool isClickLeftSide = false;

        if(mouseAngle > 90f || mouseAngle < -90f)
        {
            isClickLeftSide = true;
        }

        Vector3 currentScale = weaponEffect.localScale;
        if (!isClickLeftSide)
        {
           
            currentScale.y = -Mathf.Abs(currentScale.y); // ensure it's negative
            weaponEffect.localScale = currentScale;

            Debug.Log("try flip the weaponEffect object");
        }
        else
        {
            currentScale.y = Mathf.Abs(currentScale.y); // ensure it's not negative
            weaponEffect.localScale = currentScale;
        }

            Debug.Log("Mouse angle is: " + mouseAngle);

        // Coroutine required by method signature, but no animation here
        yield return null;
    }

    public override IEnumerator PlayLightAttackAnimation2(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        // Step 1: Get the direction from the player to the mouse
        Vector2 toMouse = mousePosition - originalPosition;

        // Step 2: Convert that direction into an angle (in degrees)
        // This angle is measured from the right (0°), going counter-clockwise
        float mouseAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;

        // Step 3: Decide which way to rotate based on mouse position
        // If mouse is on the right, rotate 90° clockwise from it
        // If mouse is on the left, rotate 90° counter-clockwise from it
        float rotateDirection = toMouse.x >= 0 ? -90f : 90f;

        // Step 4: Add the offset and normalize the angle to stay within 0–360°
        float finalAngle = (mouseAngle + rotateDirection + 360f) % 360f;

        // Step 5: Instantly rotate the weapon to that angle
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
        weaponEffect.rotation = Quaternion.Euler(0f, 0f, mouseAngle);

        bool isClickLeftSide = false;

        if (mouseAngle > 90f || mouseAngle < -90f)
        {
            isClickLeftSide = true;
        }

        Vector3 currentScale = weaponEffect.localScale;
        if (isClickLeftSide)
        {

            currentScale.y = -Mathf.Abs(currentScale.y); // ensure it's negative
            weaponEffect.localScale = currentScale;

            //Debug.Log("try flip the weaponEffect object");
        }
        else
        {
            currentScale.y = Mathf.Abs(currentScale.y); // ensure it's not negative
            weaponEffect.localScale = currentScale;
        }

        //Debug.Log("Mouse angle is: " + mouseAngle);

        // Coroutine required by method signature, but no animation here
        yield return null;
    }





    public override void HeavyAttack(int dmg, bool isCrit, float critMultiplier, Vector2 originalPosition, Vector2 mousePosition, WeaponHolder2 weaponHolder, PlayerStats playerStats)
    {

        Collider2D[] targets = Physics2D.OverlapCircleAll(originalPosition, maxRange, affectedMask);

        if(targets.Length == 0 )
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_heavy_NoHit, playerStats.transform, 1f);
            return;
        }
        else
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponSounds_heavy_Hit, playerStats.transform, 1f);
        }


            foreach (var target in targets)
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

    public override IEnumerator PlayHeavyAttackAnimation(Vector2 originalPosition, Vector2 mousePosition, Transform weaponTransform, Transform weaponEffect)
    {
        yield return null;

        weaponTransform.rotation = Quaternion.Euler(0f, 0f, -90f);

      
        Vector2 toMouse = mousePosition - originalPosition;

       
        float mouseAngle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;


        bool isClickLeftSide = false;

        if (mouseAngle > 90f || mouseAngle < -90f)
        {
            isClickLeftSide = true;
        }
        Vector3 currentScale = weaponEffect.localScale;
        if (isClickLeftSide)
        {

            currentScale.x = -Mathf.Abs(currentScale.x); // ensure it's negative
            weaponEffect.localScale = currentScale;

            //Debug.Log("try flip the weaponEffect object");
        }
        else
        {
            currentScale.x = Mathf.Abs(currentScale.x); // ensure it's not negative
            weaponEffect.localScale = currentScale;
        }
    }
}
