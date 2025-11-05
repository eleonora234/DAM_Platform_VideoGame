using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class player_Move : MonoBehaviour
{

    [Header("General Settins")]
    public float playerSpeed = 10;
    public float jumpForce = 5;
    public float wallJumpXForce = 5;
    public float wallJumpYForce = 8;
    



    [Header("Gravity Settins")]
    public float baseGravity = 2;
    public float maxFallSpeed = 10f;
    public float wallSlidemaxFallSpeed = 5f;
    public float fallspeedmultiplier = 2f;



    [Header("ground Settings")]
    public Transform groundCheckTrasform;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);


    Rigidbody2D body;

    bool leftWallCollision;
    bool rightWallCollision;
    bool isGrounded;
    float horizontalMovement = 0;
    float wallJumpXSpeed = 0;


    public LayerMask groundLayer;

    [Header("wall Settings")]
    public Transform leftWallCheckTrasform;
    public Transform rightWallCheckTrasform;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.1f);

    public LayerMask wallLayer;



    [Header("audioSFX")]
    public AudioClip jumpSFX;


    [Header("Components")]
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public AudioSource AudioSource;


    public void Update()
    {
        playerAnimator.SetFloat("Yspeed", (body.linearVelocityY));
        playerAnimator.SetFloat("Xspeed", Mathf.Abs (body.linearVelocityX) );
        if ( Mathf.Abs( body.linearVelocityX) > 0.01f)
        {
            bool needFlip = body.linearVelocityX < 0;
            playerRenderer.flipX = needFlip;
        }
         

    }

    private void SetGravity()
    {
        if (body.linearVelocityY < 0)
        {
            body.gravityScale = baseGravity * fallspeedmultiplier;

            if (leftWallCollision || rightWallCollision)
                body.linearVelocityY = Mathf.Max(body.linearVelocityY, -wallSlidemaxFallSpeed);
            else
                body.linearVelocityY = Mathf.Max(body.linearVelocityY, -maxFallSpeed);



        }
        else
            body.gravityScale = baseGravity;
    }
    
    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void FixedUpdate()
    {
        body.linearVelocityX = (horizontalMovement * playerSpeed) + wallJumpXSpeed;
        GroundCheck();
        wallCheck();
        SetGravity();
        
        if (wallJumpXSpeed != 0)
        wallJumpXSpeed *= 0.92f;
        if (Mathf.Abs(wallJumpXSpeed) < 0.01f) 
        wallJumpXSpeed = 0;
        
        if (body.linearVelocityX > 0)
        body.linearVelocityX = Mathf.Min(playerSpeed, body.linearVelocityX);
        if (body.linearVelocityX < 0)
            body.linearVelocityX = Mathf.Max(-playerSpeed, body.linearVelocityX);
    }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckTrasform.position, groundCheckSize, 0, groundLayer))

         isGrounded = true; 
        else
         isGrounded = false; 
    }


    public void wallCheck()
    {
        if (Physics2D.OverlapBox(leftWallCheckTrasform.position, wallCheckSize, 0, wallLayer))

            leftWallCollision = true;
        else
            leftWallCollision = false;

        if (Physics2D.OverlapBox(rightWallCheckTrasform.position, wallCheckSize, 0, wallLayer))

            rightWallCollision = true;
        else
            rightWallCollision = false;
    }


    public void PlayerInput_Move(CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void PlayerInput_Jump(CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                body.linearVelocityY = jumpForce;
                AudioSource.PlayOneShot(jumpSFX);

            }
            else if (rightWallCollision)
            {
                wallJumpXSpeed = -wallJumpXForce;
                body.linearVelocityY = wallJumpYForce;
                AudioSource.PlayOneShot(jumpSFX);
            }
            else if (leftWallCollision)
            {
               wallJumpXSpeed = wallJumpXForce;
                body.linearVelocityY = wallJumpYForce;
                AudioSource.PlayOneShot(jumpSFX);
            }
        }

        if (context.canceled && body.linearVelocityY >0)
        {
            body.linearVelocityY = body.linearVelocityY / 2;
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheckTrasform.position, groundCheckSize);
        Gizmos.DrawCube(leftWallCheckTrasform.position, wallCheckSize);
        Gizmos.DrawCube(rightWallCheckTrasform.position, wallCheckSize);
    }

}
