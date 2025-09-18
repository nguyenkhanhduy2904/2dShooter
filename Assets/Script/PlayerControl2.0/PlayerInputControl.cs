using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
   
    public PlayerBehaviour playerBehaviour;
    public PlayerState playerState;
    public PlayerStats playerStats;
    public PlayerAnimation playerAnimation;
    [Header("KeyMap")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode LightAttack = KeyCode.Mouse0;
    public KeyCode HeavyAttack = KeyCode.Mouse1;

    public bool movementInputAllow = true;
    public bool attackInputAllow = true; 

    private void Start()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
       
        
        
    }

    void Update()
    {
        if(movementInputAllow)
        {
            UpdateMovement();// update movement input every frame
        }
        if(attackInputAllow)
        {
            UpdateAttack();
        }
        
        
    }

    public void UpdateMovement()
    {
        if (!playerBehaviour.canMove) return;
        Vector3 moveDir = MoveToPos();
        SetFacingDirection(moveDir);
        if (moveDir != Vector3.zero)
        {
            playerBehaviour.ChangePosition(moveDir, playerStats.CurrentMoveSpeed);
            playerState.ChangeState(PlayerState.State.Move); 
        }
        else
        {
            playerState.ChangeState(PlayerState.State.Idle);
        }
    }

    public void UpdateAttack()
    {
        if (Input.GetKeyDown(LightAttack))
        {
            playerBehaviour.DoLightAttack();
            playerState.ChangeState(PlayerState.State.Attack);
        }
        if (Input.GetKeyDown(HeavyAttack))
        {
            playerBehaviour.DoHeavyAttack();
            playerState.ChangeState(PlayerState.State.Attack);
        }
    }

    public void SetFacingDirection(Vector3 direction)
    {
        // Vertical (y axis)
        if (direction.y < 0)
            playerAnimation.currentVerticalDirection = PlayerAnimation.VerticalDirection.Front;
        else if (direction.y > 0)
            playerAnimation.currentVerticalDirection = PlayerAnimation.VerticalDirection.Back;

        // Horizontal (x axis)
        if (direction.x != 0)
            playerAnimation.currentHorizontalDirection =
                (direction.x > 0) ? PlayerAnimation.HorizontalDirection.Right : PlayerAnimation.HorizontalDirection.Left;
    }


    public Vector3 MoveToPos()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey(upKey)) input += Vector3.up;
        if (Input.GetKey(downKey)) input += Vector3.down;
        if (Input.GetKey(leftKey)) input += Vector3.left;
        if (Input.GetKey(rightKey)) input += Vector3.right;

        return input;
    }
}
