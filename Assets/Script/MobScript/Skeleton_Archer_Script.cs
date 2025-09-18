using UnityEngine;
using System.Collections;


public class Skeleton_Archer_Script: EnemyBehaviour
{
    [SerializeField] GameObject bulletPrefab;
     

    public override IEnumerator ChargeAndAttack(float animLenght)
    {
        Debug.Log("Charging...");
        SoundFXManager.Instance.PlaySoundFXClip(_attackChargeUpSounds, transform, 1f);
        yield return new WaitForSeconds(animLenght);

        float dist = Vector2.Distance(transform.position, _player.transform.position);

        if (dist > _enemyRange)
        {
            ChangeState(EnemyState.Chase);
            Debug.Log("Player too run away while charging");
            yield break;
        }

        Debug.Log("Swing!");
        if (dist <= _enemyRange)
        {

            //_playerTarget.TakeDmg(_enemyAtk, false);
            FireProjectile();
            ChangeState(EnemyState.Recover);
        }
        //else
        //{
        //    Debug.Log("Player dogded!!");
        //    ChangeState(EnemyState.Chase);
        //}
        //yield return new WaitForSeconds(0.25f);

    }

    private void FireProjectile()
    {
        Vector3 dir = (_player.transform.position - transform.position).normalized;

        GameObject bulletObj = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        BulletBehaviour bullet = bulletObj.GetComponent<BulletBehaviour>();
        bullet.direction = dir;
        bullet._damage = _enemyAtk; // Or any custom damage
        bullet._isCrit = false;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
