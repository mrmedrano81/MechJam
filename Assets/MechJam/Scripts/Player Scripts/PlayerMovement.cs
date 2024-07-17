using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float moveSpeed;

    public Animator animator;
  
    float movementInput;

    private string currentState;


    //ANIMATION STATES
    const string PLAYER_IDLE = "idle";
    const string PLAYER_WALK = "player_walk";
    const string PLAYER_JUMP_FULL = "player_jump_full";
    const string PLAYER_MIDAIR = "player_MidAir";

    //float downwardInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);

        animator.SetFloat("IsMoving", Mathf.Abs(movementInput));
    }
   
    public void Move(InputAction.CallbackContext context)   
    {
        movementInput = context.ReadValue<Vector2>().x;

        //if (movementInput > 0)
        //{

        //    transform.Rotate(0f , 0f, 0f);
        //}

        //else if (movementInput < 0)
        //{
        //    transform.Rotate(new Vector3(0f, 180f, 0f));

        //}

        if (movementInput != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(context.ReadValue<Vector2>().x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }

        if (movementInput != 0)
        {
            ChangeAnimationState(PLAYER_WALK);
        }
        else
        {

            ChangeAnimationState(PLAYER_IDLE);
        }

    }

    void ChangeAnimationState(string newState)
    {
        if (newState == currentState) return;

        animator.Play(newState);

        currentState = newState;
    }
    
}
