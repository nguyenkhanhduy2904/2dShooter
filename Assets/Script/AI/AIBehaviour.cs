using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using System;

public class AIBehaviour : MonoBehaviour
{
    public AIState aiState;
    public AIStats aiStats;
    public AIAnimation aiAnimation;
    float _animationTimer;
   
    public AILerp aiLerp;
    //public float _patrolRadious;
    public GameObject target;


    [Header("Utils")]
    [SerializeField]PlayerController playerStats;
    [SerializeField] LayerMask obstacleMask;

    //Flag
    public bool isAttacking = false;
    public bool isTargetInAttackRange = false;

    //Coroutine
    public Coroutine _attackCoroutine;

     
    private void Awake()
    {

        target = GameObject.FindGameObjectWithTag("Player");
        playerStats = target.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //ProcessStateLogic(aiState);
        _animationTimer = aiAnimation.ProcessStateAnimation(aiState.currentState);
        aiAnimation.ChangeSpriteDirection(target.transform);
        //isTargetInAttackRange = CheckTargetDistance(target.transform) <= aiStats.AttackRange;
    }
    virtual public void ProcessStateLogic(AIState aiState)
    {
        switch (aiState.currentState)
        {
            case AIState.State.Idle:
                ProcessIdleLogic();
                break;
            case AIState.State.Aggro:
                ProcessAggroLogic();
                break;
            case AIState.State.InAttack:
                ProcessAttackLogic();
                break;
            case AIState.State.Die:
                ProcessDieLogic();
                break;

        }
    }



    //------IDLE------//
    public void OnEnterIdle()
    {
        aiLerp.SetPath(null);
        //Debug.Log("Enter Idle State");
    }

    public void ProcessIdleLogic()
    {
        //
    }

    public void OnExitIdle()
    {
        //
    }



    //------Aggro------//
    public void OnEnterAggro()
    {
        //
    }

    public void ProcessAggroLogic()
    {
        if(target == null)
        {
            Debug.Log("Missing _player");
            return;
        }
        if (aiLerp == null)
        {
            Debug.Log("Missing _aiLerp");
            return;
        }
        //Debug.Log(" player pos "+ target.transform.position);
        aiLerp.destination = target.transform.position;
        //Debug.Log(" destination pos" +aiLerp.destination);
        //_aiLerp.SearchPath();
       

    }

    public void OnExitAggro()
    {
        //
    }




    //------Attack------//
    public void OnEnterAttack()
    {

        isAttacking = true;


    }

    public void ProcessAttackLogic() 
    {
        //isTargetInAttackRange = CheckTargetDistance(target.transform) <= aiStats.AttackRange;
      

        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackSequence()); 
        }
        
        
    }

    public float CheckTargetDistance(Transform _target)
    {
        Vector3 _myPos = transform.position;
        Vector3 _targetPos = _target.transform.position;

        // Force z = 0 for both before measuring
        _myPos.z = 0f;
        _targetPos.z = 0f;

        float distanceToTarget = Vector3.Distance(_myPos, _targetPos);
        return distanceToTarget;
    }

    virtual public IEnumerator AttackSequence()
    {
        
        aiLerp.canMove = false;
        Debug.Log("In Charging...");
        float _waitTime = aiStats.GetAttackDelay() / 2;

        yield return new WaitForSeconds(_waitTime);
        try
        {
           

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aiStats.AttackRange);
            foreach (var hit in hits)
            {
                var target = hit.GetComponent<PlayerController>();
                if (target != null)
                {
                    int finalDamage = aiStats.AttackDamage;
                    bool isCrit = UnityEngine.Random.value <= aiStats.CritChance;
                    if (isCrit)
                        finalDamage = Mathf.RoundToInt(finalDamage * aiStats.CritMultiplier);

                    aiStats.DealDmg(target, finalDamage, isCrit);
                    Debug.Log("Hit player inside circle!");
                }
            }
        }
         
        finally
        {
          
            isAttacking = false;
            aiLerp.canMove = true;
            _attackCoroutine = null;
        }
        yield return new WaitForSeconds(_waitTime);
    }

    public void OnExitAttack()
    {

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        aiLerp.canMove = true; // <-- ensure movement restored
        isAttacking = false;


    }




    //------Die------//


    public void OnEnterDie()
    {
        //
    }


    public void ProcessDieLogic()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
       
        StartCoroutine(DieSequence());
    }

    public IEnumerator DieSequence()
    {
        // Stop movement, disable AI etc. if needed
        aiLerp.canMove = false;

        //_animator.Play("death_anim");
        //Debug.Log("Play death anim");
        GetComponent<Inventory>().InstantiateItem(transform.position);
        // Optionally: ensure the Animator updates before querying length
        //yield return null;

        //float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(_animationTimer + 0.5f);



        Debug.Log($"{aiStats.Name} has died.");




        
        Destroy(gameObject);
    }

    public void OnExitDie()
    {
        //
    }


    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSecondsRealtime(duration);
        onFinished?.Invoke();
    }

    Vector3 RandomPoint( float radious)
    {
        Vector2 rand = UnityEngine.Random.insideUnitSphere * radious;
        Vector3 targetPos = transform.position + new Vector3(rand.x, rand.y, 0f);


        var nearestNode = AstarPath.active.GetNearest(targetPos, NNConstraint.Default);
        return (Vector3)nearestNode.position; // NNInfo.position is already Vector3

    }

    public bool HasLineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 targetPos = target.transform.position;
        Vector2 dir = (targetPos - origin).normalized;
        float dist = Vector2.Distance(origin, targetPos);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, obstacleMask);

        if (hit.collider == null)
        {
            // No obstacle
            return true;
        }
        else
        {
            //Debug.Log("Line of sight blocked by: " + hit.collider.name);
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aiStats.AttackRange);
    }




}
