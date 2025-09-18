using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //public AIAnimation AIAnimation;
    public PlayerAnimation playerAnimation;
    public PlayerStats playerStats;
    public PlayerState playerState;
    public WeaponHolder2 weaponHolder;
    public bool canMove = true;
    protected float _animationTimer;

    public void ChangePosition(Vector3 direction, float speed)
    {
        Vector3 moveDirection = direction.normalized;
        Vector3 deltaMove = moveDirection * Time.deltaTime * speed;
        Vector3 newPos = transform.position + deltaMove;

        if (IsWalkable(newPos))
        {
            transform.position = newPos;
        }
        else
        {
            Debug.Log("Blocked! Cant move to obstacle");
        }
    }

    public bool IsWalkable(Vector3 position)
    {
        // Get the nearest node in the graph to this world position
        GraphNode node = AstarPath.active.GetNearest(position).node;

        // Check if the node exists and is walkable
        return node != null && node.Walkable;
    }

    public void DoLightAttack()
    {
        canMove = false;
        weaponHolder.TryAttack(WeaponHolder2.AttackType.Light);
        StartCoroutine(StartCountDown(0.25f, () => { canMove = true; }));
    }

    public void DoHeavyAttack()
    {
        weaponHolder.TryAttack(WeaponHolder2.AttackType.Heavy);
    }


    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSeconds(duration);
        onFinished?.Invoke();
    }


    public IEnumerator KnockBack(float amount, float speed, Vector2 startPos, Vector2 endPos)
    {





        yield return null;
    }


    public IEnumerator DieSequense()
    {
        yield return null;
        GetComponent<Inventory>().InstantiateItem(transform.position);



        yield return new WaitForSeconds(_animationTimer + 0.5f);
    }

    public void OnEnterIdle()
    {

    }
    public void ProcessIdleLogic()
    {

    }
    public void OnExitIdle()
    {

    }


    public void OnEnterMove()
    {

    }
    public void ProcessMoveLogic()
    {

    }
    public void OnExitMove()
    {

    }

    public void OnEnterAttack()
    {

    }
    public void ProcessAttackLogic()
    {

    }
    public void OnExitAttack()
    {

    }

    public void OnEnterStunned()
    {

    }
    public void ProcessStunnedLogic()
    {

    }
    public void OnExitStunned()
    {

    }

    public void OnEnterDie()
    {

    }
    public void ProcessDieLogic()
    {

    }
    public void OnExitDie()
    {

    }


    private void Update()
    {
        _animationTimer = playerAnimation.ProcessStateAnimation(playerState.currentState);
    }


}
