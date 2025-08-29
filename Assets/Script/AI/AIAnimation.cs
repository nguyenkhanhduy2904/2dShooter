using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    public Animator animator;
    public AIState aiState;
    public AIStats AIStats;
    public SpriteRenderer spriteRenderer;

    public float PlayAnimation(string name)
    {
        animator.Play(name);
        float animLenght = animator.GetCurrentAnimatorStateInfo(0).length;
        return animLenght;
    }

    public float ProcessStateAnimation(AIState.State state)
    {
        float time = 0f;
       
        switch (state) 
        {
            case AIState.State.Idle:
                time = PlayAnimation("idle_anim");
                animator.speed = 1f;
                break;
            case AIState.State.Aggro:
                time = PlayAnimation("move_anim");
                animator.speed = 1f;
                break;
            case AIState.State.InAttack:
                float _animationSpeed = Mathf.Max(AIStats.AttackSpeed, 1f);
                time = PlayAnimation("attack_anim");
                animator.speed = _animationSpeed;
                break;
            case AIState.State.Die:
                time = PlayAnimation("death_anim");
                animator.speed = 1f;
                break;
        }
        return time;
    }

    public void ChangeSpriteDirection(Transform target)
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;

        if(dir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


    }
}
