using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookShot : MonoBehaviour
{
    [SerializeField] private GameObject grapple;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform firePoint;


    //[SerializeField] private float hookLength;
    //[SerializeField] private LayerMask hookLayer;


    //[SerializeField] private NanoBotInputs nanoBotInput;

    //private Vector3 hookPoint;
    //[SerializeField] private DistanceJoint2D joint;

    private Vector3 mousePos;

    //private InputAction fireGrapple;

    //private void Awake()
    //{
    //    nanoBotInput = new NanoBotInputs();
    //}

    //private void OnEnable()
    //{
    //    fireGrapple = nanoBotInput.Player.Grapple;
    //    fireGrapple.Enable();
    //    fireGrapple.performed += Grapple;
    //}

    //private void OnDisable()
    //{
    //    fireGrapple.Disable();
    //}
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //joint = GetComponent<DistanceJoint2D>();
        //joint.enabled = false;
    }
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rotation = (mousePos - transform.position).normalized;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        rb.rotation = rotZ;

        if (Input.GetMouseButtonDown(0)) {
            var hook = Instantiate(grapple, firePoint.position, firePoint.rotation);
            hook.GetComponent<Grapplehook>().caster = transform;
        }

        Debug.DrawRay(firePoint.position, mousePos, Color.red);
    }

    public void Grapple(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed");
        
        //if (context.performed)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(
        //    transform.position,
        //    Camera.main.ScreenToWorldPoint(Input.mousePosition),
        //    Mathf.Infinity,
        //    hookLayer
        //    );

        //    if (hit.collider != null)
        //    {
        //        hookPoint = hit.point;
        //        hookPoint.z = 0;
        //        joint.connectedAnchor = hookPoint;
        //        joint.enabled = true;
        //        joint.distance = hookLength;
        //    }
        //}

        //else if (context.canceled)
        //{
        //    joint.enabled = false;
        //}
    }
}
