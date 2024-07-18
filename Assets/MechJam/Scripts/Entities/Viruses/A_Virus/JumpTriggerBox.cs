using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerBox : MonoBehaviour
{
    public bool redBloodCellInJumpRange;
    public Transform redBloodCellTransform;
    public bool targetAcquired;

    // Start is called before the first frame update
    void Start()
    {
        targetAcquired = false;
        redBloodCellTransform = null;
        redBloodCellInJumpRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RedBloodCell"))
        {
            redBloodCellInJumpRange = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trigger: "+collision.gameObject.name);

        if (collision.gameObject.CompareTag("RedBloodCell") && targetAcquired == false)
        {
            redBloodCellTransform = collision.transform;
            redBloodCellInJumpRange = true;
            targetAcquired = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RedBloodCell"))
        {
            redBloodCellInJumpRange = false;
        }
    }
}
