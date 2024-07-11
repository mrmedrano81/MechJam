using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lasering : MonoBehaviour
{
    [SerializeField] private LayerMask hitSomething;

    [SerializeField] private NanoBotInputs nanoBotInput;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform firePoint;

     private InputAction fire;

    private void OnEnable()
    {
        fire = nanoBotInput.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    private void Awake()
    {
        nanoBotInput = new NanoBotInputs();
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousPos, hitSomething);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, mousPos);


            if (hit.collider != null)
            {
                //do damage to blocks
            }
        }
    }
}
