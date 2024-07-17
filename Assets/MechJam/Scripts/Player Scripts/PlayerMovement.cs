using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float moveSpeed;

    Animator animator;
  
    float movementInput;
    //float downwardInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        if (movementInput > 0)
        {
            transform.localScale = new Vector3(5f, 5f, 1f);
        }

        else if(movementInput < 0)
        {
            transform.localScale = new Vector3(-5f,5f,1f);
        }

    }

    
}
