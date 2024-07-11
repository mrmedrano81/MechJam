using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplehook : MonoBehaviour
{
    Rigidbody2D rb;

    [Range(1, 2000)]
    [SerializeField] private int grappleSpeed;


    [SerializeField] private float range, rangeMax;

    [HideInInspector] 
    public Transform caster;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * grappleSpeed * Time.deltaTime);

        // rb.AddForce(transform.right  * Time.deltaTime);
        

        var dist = Vector2.Distance(transform.position, caster.position);

        if (dist > range)
        {
            //Destroy(gameObject);
            //transform.LookAt(caster, Vector3.zero);

        }

    }
}
