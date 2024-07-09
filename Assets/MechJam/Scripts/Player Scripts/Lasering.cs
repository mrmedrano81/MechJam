using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasering : MonoBehaviour
{
    [SerializeField] private LayerMask hitSomething;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), hitSomething);

        //    if (hit.collider != null)
        //    {
        //        Debug.Log("PLatform!");
        //    }
        //}
    }
}
