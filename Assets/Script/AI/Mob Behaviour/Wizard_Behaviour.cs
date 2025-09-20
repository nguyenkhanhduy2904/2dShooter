using Pathfinding;
using System.Collections;
using UnityEngine;


public class Wizard_Behaviour: AIBehaviour
{
    public float teleportActiveRange = 2f;
    public float teleportDistance = 4.5f;
    public float teleportCoolDown = 5f;
    public float teleportCoolDownRemain = 0f;
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
            yield return new WaitForSeconds(_animationTimer *0.5f);
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

        Fireball_Script _bulletBehaviour = _projectileObj.GetComponent<Fireball_Script>();
        _bulletBehaviour.layerMask = layerMask;
        _bulletBehaviour.direction = _shootDirection;
        _bulletBehaviour._damage = aiStats.AttackDamage;
        _bulletBehaviour._isCrit = false;

        float angle = Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg;
        _projectileObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public override void Update()
    {
        base.Update();

        if(teleportCoolDownRemain > 0f)
        {
            teleportCoolDownRemain -= Time.deltaTime;
        }


        if (CheckTargetDistance(target.transform) <= teleportActiveRange && teleportCoolDownRemain <= 0f )

        {
            StartCoroutine(TeleportSequence(_animationTimer *2 + 0.15f));
            teleportCoolDownRemain = teleportCoolDown;
           
            //aiAnimation.ClearOverrideAnimation();
        }
    }

    public IEnumerator TeleportSequence(float reappearDelay)
    {
        if(_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        //StartCoroutine(aiState.ForceInState(AIState.State.Idle, reappearDelay + _animationTimer));

        aiState.ForceInState(AIState.State.Idle, reappearDelay + _animationTimer);
        aiAnimation.PlayOverrideAnimation("teleport_anim");

        collider2D.enabled = false;
        yield return new WaitForSeconds(_animationTimer);
        aiAnimation.ClearOverrideAnimation();
       
        //spriteRenderer.enabled = false;
        bool isTeleportPosSet = false;
        var tempPos = transform.position;
        while (!isTeleportPosSet)
        {
            tempPos = PickRandomPos(transform.position, teleportDistance);
            if (IsWalkable(tempPos)) 
            { 
               
                isTeleportPosSet = true;
            }
        }
        aiLerp.Teleport(tempPos);
        aiLerp.SetPath(null);
        aiAnimation.PlayOverrideAnimation("teleport_reverse_anim");
        yield return new WaitForSeconds( reappearDelay);
        aiAnimation.ClearOverrideAnimation();
        collider2D.enabled = true;
        //spriteRenderer.enabled = true;

    }

    public bool IsWalkable(Vector3 position)
    {
        // Get the nearest node in the graph to this world position
        GraphNode node = AstarPath.active.GetNearest(position).node;

        // Check if the node exists and is walkable
        return node != null && node.Walkable;
    }

    public Vector3 PickRandomPos(Vector3 currentPos, float distance)
    {
        // Random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Random point in a circle
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        Vector3 candidatePos = target.transform.position + (Vector3)offset;

        return candidatePos;
    }
}
