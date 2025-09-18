using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    //public AIState aiState;
    //public AIStats AIStats;
    public SpriteRenderer charSpriteRenderer;
    public SpriteRenderer weaponSpriteRenderer;
    public string overrideAnim = null;
    public string currentPlayingAnim = "idle_anim";
    
    public enum VerticalDirection
    {
        Front,
        Back
    }
    public enum HorizontalDirection
    {
        Right,
        Left
    }

    public VerticalDirection currentVerticalDirection = VerticalDirection.Front ;
    public HorizontalDirection currentHorizontalDirection = HorizontalDirection.Right ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetSpriteDirection();
    }

    public void SetAnimationName(PlayerState.State state, VerticalDirection verticalDirection)
    {
        string animationName = currentPlayingAnim;
        string vertical = "front";
        string action = "idle";
        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogWarning("animationName string was empty");
            return;
        }
        if (verticalDirection == VerticalDirection.Front)
        {
            vertical = "front";
        }
        else
        {
            vertical = "back";
        }

        switch (state)
        {
            case PlayerState.State.Idle:
                action = "idle";
                break;
            case PlayerState.State.Move:
                action = "move";
                break;
            case PlayerState.State.Attack:
                action = "action";
                break;
            case PlayerState.State.Stunned:
                action = "idle";
                break;// temporary gonna set the idle animation to be the same as idle
            case PlayerState.State.Die:
                action = "die";
                break;
        }

        animationName = action + "_" + vertical;
        //Debug.Log("Animation Name is: " + animationName);
        currentPlayingAnim = animationName;
        
    }

    public float ProcessStateAnimation(PlayerState.State state)
    {
        if (!string.IsNullOrEmpty(overrideAnim))
        {
            // skip normal state animation
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }
        float time = 0f;

        switch (state)
        {
            case PlayerState.State.Idle:
                SetAnimationName(state, currentVerticalDirection);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case PlayerState.State.Move:
                SetAnimationName(state, currentVerticalDirection);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case PlayerState.State.Attack:
                //float _animationSpeed = Mathf.Max(AIStats.AttackSpeed, 1f);
                SetAnimationName(state, currentVerticalDirection);
                time = PlayAnimation(currentPlayingAnim);
                //animator.speed = 1 / AIStats.GetAttackDelay();
                animator.speed = 1f;
                //Debug.Log("animation speed: " + animator.speed);
                break;
            case PlayerState.State.Stunned:
                SetAnimationName(state, currentVerticalDirection);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
            case PlayerState.State.Die:
                SetAnimationName(state, currentVerticalDirection);
                time = PlayAnimation(currentPlayingAnim);
                animator.speed = 1f;
                break;
        }
        return time;
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

    public float PlayAnimation(string name)
    {
        animator.Play(name);
        float animLenght = animator.GetCurrentAnimatorStateInfo(0).length;
        return animLenght;
    }

    public void SetSpriteDirection()
    {
        if(currentHorizontalDirection == HorizontalDirection.Left)
        {
            charSpriteRenderer.flipX = true;
          
           

        }
        else
        {
            charSpriteRenderer.flipX = false;
           


        }
    }
}
