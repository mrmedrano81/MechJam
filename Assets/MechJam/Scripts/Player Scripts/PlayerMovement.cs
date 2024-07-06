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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
        
    }
   
    void OnMove2(InputValue value)
    {
        movementInput = value.Get<float>();
    }
    
}
