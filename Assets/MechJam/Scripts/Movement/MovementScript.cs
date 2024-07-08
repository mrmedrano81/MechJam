using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void faceDirection(Transform target)
    {
        // Calculate the rotation angle as before
        Vector2 lookDir = target.position - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime);
    }
}
