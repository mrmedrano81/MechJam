using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private int jumpingPower;

    public bool detectGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnJump()
    {
        
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        
    }
}
