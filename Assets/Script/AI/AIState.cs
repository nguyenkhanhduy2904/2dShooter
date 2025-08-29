using System;
using System.Xml.Serialization;
using UnityEngine;

public class AIState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Aggro,
        InAttack,
   
        Die
    }

    public State currentState;

    Action onEnter;
    Action onUpdate;
    Action onExit;


    public AIBehaviour aiBehaviour;
    public AIStats aiStats;
    public GameObject _target;

    public void ChangeState( State state)
    {
        //var oldState = currentState;

        onExit?.Invoke();//call the exit method of the previous state

        currentState = state;

        switch (state)
        {
            case State.Idle:
                onEnter = aiBehaviour.OnEnterIdle;
                onUpdate = aiBehaviour.ProcessIdleLogic;
                onExit = aiBehaviour.OnExitIdle;
                break;
            case State.Aggro:
                onEnter = aiBehaviour.OnEnterAggro;
                onUpdate = aiBehaviour.ProcessAggroLogic;
                onExit = aiBehaviour.OnExitAggro;
                break;
            case State.InAttack:
                onEnter = aiBehaviour.OnEnterAttack;
                onUpdate = aiBehaviour.ProcessAttackLogic;
                onExit = aiBehaviour.OnExitAttack;
                break;
            case State.Die:
                onEnter = aiBehaviour.OnEnterDie;
                onUpdate = aiBehaviour.ProcessDieLogic;
                onExit = aiBehaviour.OnExitDie;
                break;

        }

        onEnter?.Invoke();//call the enter method of the new state

        
    }
    private void Start()
    {
        _target = aiBehaviour.target;
    }

    private void Update()
    {
        //Debug.Log("current State is" + currentState);
        Vector3 pos = transform.position;
        Vector3 playerPos = _target.transform.position;

        // Force z = 0 for both before measuring
        pos.z = 0f;
        playerPos.z = 0f;

        float distanceToTarget = Vector3.Distance(pos, playerPos);
        
        switch ( currentState)
        {
            case State.Idle:

                onUpdate?.Invoke();

                if (aiBehaviour.HasLineOfSight() || distanceToTarget <= aiStats.AggroRange)
                {
                    ChangeState(State.Aggro);
                    
                }
                break;

            case State.Aggro:
                onUpdate?.Invoke();
                //Debug.Log("distant to target: " + distanceToTarget);
                //Debug.Log("Attack range: " + aiStats.AttackRange);
                if (distanceToTarget <= aiStats.AttackRange)
                {
                   ChangeState(State.InAttack);
                }
                break;
            case State.InAttack:
                //Debug.Log("target was out of distant: " + (distanceToTarget > aiStats.AttackRange));
                //Debug.Log("isAttacking was: "+(aiBehaviour.isAttacking));
                onUpdate?.Invoke();

                if (!aiBehaviour.isAttacking)
                {
                    ChangeState(State.Idle);
                }

               
                break;
        }

    }
}
