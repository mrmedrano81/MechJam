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

   // public bool detectGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Gravity();
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
}
