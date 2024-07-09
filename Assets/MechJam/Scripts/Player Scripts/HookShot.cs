using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookShot : MonoBehaviour
{
    [SerializeField] private float hookLength;
    [SerializeField] private LayerMask hookLayer;


    [SerializeField] private NanoBotInputs nanoBotInput;

    private Vector3 hookPoint;
    [SerializeField] private DistanceJoint2D joint;

    private InputAction fire;

    //private void Awake()
    //{
    //    nanoBotInput = new NanoBotInputs();
    //}

    //private void OnEnable()
    //{
    //    fire = nanoBotInput.Player.Fire;
    //    fire.Enable();
    //    fire.performed += Grapple;
    //}

    //private void OnDisable()
    //{
    //    fire.Disable();
    //}
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;

    }
    private void Update()
    {
        Debug.DrawRay(transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse1");
            RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Mathf.Infinity,
            hookLayer
            );

            if (hit.collider != null)
            {
                Debug.Log("Hit!");
                hookPoint = hit.point;
                hookPoint.z = 0;
                joint.connectedAnchor = hookPoint;
                joint.enabled = true;
                joint.distance = hookLength;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("mouse2");
            joint.enabled = false;
        }
    }

    //public void Grapple(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Pressed");
    //    //if (context.performed)
    //    //{
    //        RaycastHit2D hit = Physics2D.Raycast(
    //            origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
    //            direction: Vector2.zero,
    //            distance: Mathf.Infinity,
    //            layerMask: hookLayer
    //            );

    //        if (hit.collider != null)
    //        {
    //            hookPoint = hit.point;
    //            hookPoint.z = 0;
    //            joint.connectedAnchor = hookPoint;
    //            joint.enabled = true;
    //            joint.distance = hookLength;
    //        }
    //    //}

    //    //else if (context.canceled)
    //    //{
    //    //    joint.enabled = false;
    //    //}
    //}    
}
