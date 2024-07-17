using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [SerializeField] private float baseGravity;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fallSpeedMultiplier;

    [SerializeField] private float coyoteTime = 0.2f;
     private float coyoteTimeCounter;

    [SerializeField] private int jumpingPower;

    Animator _animator;
    private string currentState;

    //ANIMATION STATE
    const string PLAYER_WALK = "player_walk";
    const string PLAYER_IDLE = "idle";
    const string PLAYER_MIDAIR = "player_MidAir";
    const string PLAYER_JUMP_ONLY = "player_JumpOnly";
    const string PLAYER_JUMP_LANDING = "player_JumpLanding";
    public bool detectGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Gravity();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ChangeAnimationState(PLAYER_JUMP_ONLY);
        //}


        //if (!isGrounded())
        //{
        //    ChangeAnimationState(PLAYER_MIDAIR);
        //}

        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        

        if (context.performed && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        else if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
        
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }

        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }
    void ChangeAnimationState(string newState)
    {
        if (newState == currentState) return;

        _animator.Play(newState);

        currentState = newState;
    }

    //A bool checker to check if animation is still playing
    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
