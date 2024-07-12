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

    private Vector3 mousePos;

    Rigidbody2D rb;

     private InputAction fire;

    //private void OnEnable()
    //{
    //    fire = nanoBotInput.Player.Fire;
    //    fire.Enable();
    //    fire.performed += Fire;
    //}

    //private void OnDisable()
    //{
    //    fire.Disable();
    //}

    //private void Awake()
    //{
    //    nanoBotInput = new NanoBotInputs();
    //}
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rotation = (mousePos - transform.position).normalized;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        rb.rotation = rotZ;


        if (Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, hitSomething);

            LaserUpdate();

            if (hit.collider != null)
            {
                //do damage to blocks
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
        }
    }

    //private void Fire(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousPos, hitSomething);

    //        lineRenderer.SetPosition(0, firePoint.position);
    //        lineRenderer.SetPosition(1, mousPos);


    //        if (hit.collider != null)
    //        {
    //            //do damage to blocks
    //        }
    //    }
    //}

    void LaserUpdate()
    {
        var mousPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, mousPos);
    }
}
