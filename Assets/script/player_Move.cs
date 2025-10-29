using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class player_Move : MonoBehaviour
{

    [Header("General Settins")]
    public float playerSpeed = 10;
    public float jumpForce = 5;




    [Header("Gravity Settins")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallspeedmultiplier = 2f;



    [Header("ground Settings")]
    public Transform groundCheckTrasform;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);


    Rigidbody2D body;

    bool isGrounded;
    float horizontalMovement = 0;


    public LayerMask groundLayer;

    [Header("Components")]
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;



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
            body.linearVelocityY = Mathf.Max(body.linearVelocityY, -maxFallSpeed);
        }   
    }
    
    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void FixedUpdate()
    {
        body.linearVelocityX = horizontalMovement * playerSpeed;
        GroundCheck();
    }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckTrasform.position, groundCheckSize, 0, groundLayer))

         isGrounded = true; 
        else
         isGrounded = false; 
    }


    public void PlayerInput_Move(CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void PlayerInput_Jump(CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.performed)
            {
                body.linearVelocityY = jumpForce;

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
    }

}
