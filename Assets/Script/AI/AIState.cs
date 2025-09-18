using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class AIState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Aggro,
        InAttack,
        Stunned,
   
        Die
    }

    public State currentState;

    Action onEnter;
    Action onUpdate;
    Action onExit;

    public AIAnimation aiAnimation;
    public AIBehaviour aiBehaviour;
    public AIStats aiStats;
    public GameObject _target;
    public Coroutine forceInState = null;
    [Header("Flag")]
    public bool isInForceState = false;


    

    public void ChangeState(State state)
    {
        //var oldState = currentState;
        if (isInForceState && state != State.Die) return;

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
            case State.Stunned:
                onEnter = aiBehaviour.OnEnterStunned;
                onUpdate = aiBehaviour.ProcessStunnedLogic;
                onExit = aiBehaviour.OnExitStunned;
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

        if (isInForceState) return;



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
                //Debug.Log("Has line of sight: " + aiBehaviour.HasLineOfSight());
                //Debug.Log("Distance to Target <= aiStats.AggroRange : " + (distanceToTarget <= aiStats.AggroRange));
                //Debug.Log("isAttackingAllow: " + aiBehaviour.isAttackingAllow);
                Debug.Log("AggroRange: " + aiStats.AggroRange);
                Debug.Log("distanceToTarget: " + distanceToTarget);
                if ((aiBehaviour.HasLineOfSight() && distanceToTarget <= aiStats.AggroRange )&& aiBehaviour.isAttackingAllow)
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
            case State.Stunned:
                onUpdate?.Invoke();
                ChangeState(State.Aggro);
                break;
            case State.Die:
                onUpdate?.Invoke();
                break;
        }

    }

    //public void ApplyForceCoroutine(State state, float duration)
    //{
    //    if (forceInState != null)
    //    {
    //        StopCoroutine(forceInState);
    //    }
    //    forceInState = StartCoroutine(ForceInState(state, duration));
    //}

    //public IEnumerator ForceInState(State state, float duration)
    //{
    //    if (currentState == state)
    //    {
    //        yield return new WaitForSeconds(duration);

    //        forceInState = null; // clear the reference
    //    }
    //    else
    //    {
    //        ChangeState(state);

    //        yield return new WaitForSeconds(duration);

    //        forceInState = null; // clear the reference
    //    }





    //}

    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSecondsRealtime(duration);
        onFinished?.Invoke();
    }

    public void ForceInState(State state, float duration)
    {
        //aiAnimation.PlayAnimation(aiAnimation.currentPlayingAnim);
        aiAnimation.animator.Play(aiAnimation.currentPlayingAnim, 0, 0f);
        






        if (currentState == state)
        {
            isInForceState = true;
            if(forceInState == null)
            {
                forceInState = StartCoroutine(StartCountDown(duration, () => isInForceState = false));
            }
            else
            {
                StopCoroutine(forceInState);
                forceInState = StartCoroutine(StartCountDown(duration, () => isInForceState = false));
            }
        }

        else if(currentState != state)
        {
            ChangeState(state);
            isInForceState = true;
            if (forceInState == null)
            {
                forceInState = StartCoroutine(StartCountDown(duration, () => isInForceState = false));
            }
            else
            {
                StopCoroutine(forceInState);
                forceInState = StartCoroutine(StartCountDown(duration, () => isInForceState = false));
            }
        }
           
            
    }

    




}
