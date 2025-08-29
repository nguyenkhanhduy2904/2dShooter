using UnityEngine;
using System.Collections;

public class Wizard_Behaviour: AIBehaviour
{
    public GameObject projectilePrefab;
    public override IEnumerator AttackSequence()
    {



        aiLerp.canMove = false;
        float _waitTime = aiStats.GetAttackDelay() / 2;
        yield return new WaitForSeconds(_waitTime);
        try
        {
            FireProjectile();
        }
        finally
        {
            isAttacking = false;
            aiLerp.canMove = true;
            _attackCoroutine = null;
        }

        yield return new WaitForSeconds(_waitTime);
    }

    public void FireProjectile()
    {
        Vector2 _shootDirection = (target.transform.position - transform.position).normalized;

        GameObject _projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Fireball_Script _bulletBehaviour = _projectileObj.GetComponent<Fireball_Script>();

        _bulletBehaviour.direction = _shootDirection;
        _bulletBehaviour._damage = aiStats.AttackDamage;
        _bulletBehaviour._isCrit = false;

        float angle = Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg;
        _projectileObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
