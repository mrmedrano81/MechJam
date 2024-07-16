using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lasering : MonoBehaviour
{
    [SerializeField] private LayerMask hitSomething;

    [SerializeField] private NanoBotInputs nanoBotInput;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform laserFirePoint;

    [SerializeField] private Transform armRotation;

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
        Debug.DrawRay(laserFirePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        Vector2 laserLookDir = (mousePos - laserFirePoint.position);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rotation = (mousePos - armRotation.position).normalized;

        //float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;


        if (Input.GetMouseButton(0))
        {
            //armRotation.rotation = Quaternion.Euler(0f, 0f, rotZ);

            lineRenderer.enabled = true;

            RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, mousePos, hitSomething);

            LaserUpdate();

            if (hit.collider != null)
            {
                //do damage to blocks
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
           // armRotation.rotation = Quaternion.Euler(0f, 0f, 0f);
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

        lineRenderer.SetPosition(0, laserFirePoint.position);
        lineRenderer.SetPosition(1, mousPos);
    }
}
