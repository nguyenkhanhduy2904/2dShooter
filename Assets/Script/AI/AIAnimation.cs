using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class AIAnimation : MonoBehaviour
{
    public Animator animator;
    public AIState aiState;
    public AIStats AIStats;
    public SpriteRenderer spriteRenderer;
    public string overrideAnim = null;
    public string currentPlayingAnim = "idle_anim";

    

    public float PlayAnimation(string name)
    {
        animator.Play(name);
        float animLenght = animator.GetCurrentAnimatorStateInfo(0).length;
        return animLenght;
    }

    public void SetAnimationName(AIState.State state)
    {
        
        if(state == AIState.State.Idle)
        {
            currentPlayingAnim = "idle_anim";
        }
        else if (state == AIState.State.Aggro)
        {
            currentPlayingAnim = "move_anim";
        }
        else if(state == AIState.State.InAttack)
        {
            currentPlayingAnim = "attack_anim";
        }
        else if (state == AIState.State.Stunned)
        {
            currentPlayingAnim = "hurt_anim";
        }
        else if(state == AIState.State.Die)
        {
            currentPlayingAnim = "death_anim";
        }
    }

    public float ProcessStateAnimation(AIState.State state)
    {
        if (!string.IsNullOrEmpty(overrideAnim))
        {
            // skip normal state animation
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }
        float time = 0f;
       
        switch (state) 
        {
            case AIState.State.Idle:
                SetAnimationName(state);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case AIState.State.Aggro:
                SetAnimationName(state);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case AIState.State.InAttack:
                //float _animationSpeed = Mathf.Max(AIStats.AttackSpeed, 1f);
                SetAnimationName(state);
                time = PlayAnimation(currentPlayingAnim);
                //animator.speed = 1 / AIStats.GetAttackDelay();
                animator.speed = 1f;
                Debug.Log("animation speed: " + animator.speed);
                break;
            case AIState.State.Stunned:
                SetAnimationName(state);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case AIState.State.Die:
                SetAnimationName(state);
                time = PlayAnimation(currentPlayingAnim);
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

    public void PlayOverrideAnimation(string animName, float speed = 1f, bool reverse = false)
    {
        overrideAnim = animName;

        if (reverse)
        {
            animator.Play(animName, 0, 0.999f);
            animator.Update(0f); // force Animator to apply
            animator.speed = -Mathf.Abs(speed);

        }
        else
        {
            animator.speed = Mathf.Abs(speed);
            animator.Play(animName, 0, 0f);
        }
    }



    public void ClearOverrideAnimation()
    {
        overrideAnim = null;
    }
}
