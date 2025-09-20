using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SKeletonArcher_Behaviour: AIBehaviour
{
    public GameObject projectilePrefab;
    public LayerMask layerMask;
    public override IEnumerator AttackSequence()
    {
        aiLerp.canMove = false;
        //float _waitTime = aiStats.GetAttackDelay()/2;
        float cooldown = Mathf.Max(0.1f, 1f / aiStats.AttackSpeed);
        //Debug.Log("wait time is: " +  _waitTime);
        try
        {
            yield return new WaitForSeconds(_animationTimer * 0.5f);
            FireProjectile();
            isAttackingAllow = false;
            yield return new WaitForSeconds(_animationTimer * 0.5f);
            isAttacking = false;
            aiLerp.canMove = true;
            _attackCoroutine = null;
            yield return new WaitForSeconds(cooldown);
            isAttackingAllow = true;
        }
        finally
        {
            isAttackingAllow = true;
            isAttacking = false;
            aiLerp.canMove = true;
            _attackCoroutine = null;
        }

        //yield return new WaitForSeconds(_waitTime);
    }

    public void FireProjectile()
    {
        Vector2 _shootDirection = (target.transform.position - transform.position).normalized;

        GameObject _projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        BulletBehaviour _bulletBehaviour = _projectileObj.GetComponent<BulletBehaviour>();
        _bulletBehaviour.layerMask = layerMask;
        _bulletBehaviour.direction = _shootDirection;
        _bulletBehaviour._damage = aiStats.AttackDamage;
        _bulletBehaviour._isCrit = false;

        float angle = Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg;
        _projectileObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }


}
