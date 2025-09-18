using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
   public enum State
    {
        Idle,
        Move,
        Attack,
        Stunned,
        Die
    }

    Action onEnter;
    Action onUpdate;
    Action onExit;
    public PlayerBehaviour playerBehaviour;

    public State currentState;


    public void ChangeState(State state)
    {
        onExit?.Invoke();

        currentState = state;

        switch (state)
        {
            case State.Idle:
                onEnter = playerBehaviour.OnEnterIdle;
                onUpdate = playerBehaviour.ProcessIdleLogic;
                onExit = playerBehaviour.OnExitIdle;
                break;

            case State.Move:
                onEnter = playerBehaviour.OnEnterMove;
                onUpdate = playerBehaviour.ProcessMoveLogic;
                onExit = playerBehaviour.OnExitMove;
                break;
            case State.Attack:
                onEnter = playerBehaviour.OnEnterAttack;
                onUpdate = playerBehaviour.ProcessAttackLogic;
                onExit = playerBehaviour.OnExitAttack;
                break;
            case State.Stunned:
                onEnter = playerBehaviour.OnEnterStunned;
                onUpdate = playerBehaviour.ProcessStunnedLogic;
                onExit = playerBehaviour.OnExitStunned;
                break;
            case State.Die:
                onEnter = playerBehaviour.OnEnterDie;
                onUpdate = playerBehaviour.ProcessDieLogic;
                onExit = playerBehaviour.OnExitDie;
                break;

        }
        onExit?.Invoke();

    }

    private void Update()
    {
        onUpdate?.Invoke();
    }
}
