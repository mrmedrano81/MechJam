using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float moveSpeed;

  
    float movementInput;
    //float downwardInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);

    }
   
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>().x;
    }
    
    
}
