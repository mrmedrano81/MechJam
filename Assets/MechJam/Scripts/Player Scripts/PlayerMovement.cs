using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Range(0f, 10f)]
    [SerializeField] private float moveSpeed;

    [SerializeField] NanoBotInputs nanobotInputs;

    Vector2 moveDirection = Vector2.zero;

    private InputAction move;
    
    private void OnEnable()
    {
        move = nanobotInputs.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Awake()
    {
        nanobotInputs = new NanoBotInputs();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, 0);
    }
}
