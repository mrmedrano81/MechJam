using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookShot : MonoBehaviour
{
    //[SerializeField] private GameObject grapple;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform firePoint;


    [SerializeField] private float hookDistance;
    [SerializeField] private float hookLength;
    [SerializeField] private LayerMask hookLayer;


    private Vector3 hookPoint;
    [SerializeField] private DistanceJoint2D joint;

    private Vector3 mousePos;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }
    private void Update()
    {
        Vector2 lookDir = (mousePos - firePoint.position);
        Debug.DrawRay(firePoint.position, lookDir, Color.red);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 rotation = (mousePos - transform.position).normalized;

        //float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        //rb.rotation = rotZ;

        if (Input.GetMouseButton(1))
        {
            //var hook = Instantiate(grapple, firePoint.position, firePoint.rotation);
            //hook.GetComponent<Grapplehook>().caster = transform;

            RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            lookDir,
            hookDistance,
            hookLayer
            );

            if (hit.collider != null)
            {
                hookPoint = hit.point;
                hookPoint.z = 0;
                joint.connectedAnchor = hookPoint;
                joint.enabled = true;
                joint.distance = hookLength;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            joint.enabled = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hookDistance);
    }


}
