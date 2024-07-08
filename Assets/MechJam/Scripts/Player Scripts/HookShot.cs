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
    private DistanceJoint2D joint;
    private InputAction fire;

    private void Awake()
    {
        nanoBotInput = new NanoBotInputs();
    }

    private void OnEnable()
    {
        fire = nanoBotInput.Player.Fire;
        fire.Enable();
        fire.performed += Grapple;
    }

    private void OnDisable()
    {
        fire.Disable();
    }
    private void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;

    }

    public void Grapple(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed");
        //if (context.performed)
        //{
            RaycastHit2D hit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector2.zero,
                distance: Mathf.Infinity,
                layerMask: hookLayer
                );

            if (hit.collider != null)
            {
                hookPoint = hit.point;
                hookPoint.z = 0;
                joint.connectedAnchor = hookPoint;
                joint.enabled = true;
                joint.distance = hookLength;
            }
        //}

        //else if (context.canceled)
        //{
        //    joint.enabled = false;
        //}
    }    
}
