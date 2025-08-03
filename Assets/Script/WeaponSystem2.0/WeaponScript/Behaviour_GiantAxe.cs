using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponBehaviour/Giant Axe")]
public class Behaviour_GiantAxe: WeaponBehaviour
{
    [SerializeField] public GameObject sliceEffectPrefab;
   
    [Range(0f, 360f)]
    public float swingArc_light = 60f;
    [Range(0f, 360f)]
    public float swingArc_heavy = 360f;
    public float maxRange = 5f;
    public LayerMask affectedMask;
    public override IEnumerator LightAttack(int dmg, bool isCrit, PlayerController _player)
    {
        float duration = 0.1f;
        float elapsed = 0f;

        float arc = swingArc_light;
        float startAngle = -arc / 2;
        float endAngle = arc / 2;

        Transform weaponSprite = _player.weaponHolder2.weaponPivot;
        Vector3 aimDir = (_player.weaponHolder2.GetMousePositionInWorld() - _player.transform.position).normalized;
        float baseRotation = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        //  Snap to starting angle immediately
        weaponSprite.rotation = Quaternion.Euler(0, 0, baseRotation + startAngle);
       
       
        //  Animate weapon swing with easing
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = t * t * t; // or use SmoothStep for smoother feel
            float angle = Mathf.Lerp(startAngle, endAngle, easedT);

            weaponSprite.rotation = Quaternion.Euler(0, 0, baseRotation + angle);
            yield return null;
        }

        //  Ensure it ends exactly at final angle
        weaponSprite.rotation = Quaternion.Euler(0, 0, baseRotation + endAngle);

        // Do damage after swing is complete
        Vector3 origin = _player.transform.position;
        //debug
        float arcStep = 10f;
        for (float a = -swingArc_light / 2; a <= swingArc_light / 2; a += arcStep)
        {
            Vector3 dir = Quaternion.Euler(0, 0, baseRotation + a) * Vector3.up;
            Debug.DrawLine(origin, origin + dir * maxRange, Color.red, 0.1f);
        }


        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, maxRange, affectedMask);
        foreach (var hit in hits)
        {
            Vector3 toTarget = ((Vector3)hit.transform.position - origin).normalized;
            float angle = Vector2.Angle(aimDir, toTarget);

            if (angle <= swingArc_light / 2f)
            {
                if (hit.TryGetComponent<EnemyBehaviour>(out var enemy))
                {
                    _player.DealDmg(enemy, dmg, isCrit);
                    
                    enemy.KnockBack(_player.transform, 0.1f, 1f);
                    Debug.Log("Light attack called");
                }
            }
        }
        
    }



    public override IEnumerator HeavyAttack(int dmg, bool isCrit, PlayerController _player)
    {
        Debug.Log(this.name + "heavy attack called");
        yield return null;
    }
}
