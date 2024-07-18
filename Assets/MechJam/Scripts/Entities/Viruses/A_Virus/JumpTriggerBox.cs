using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerBox : MonoBehaviour
{
    public bool redBloodCellInJumpRange;
    public Transform redBloodCellTransform;

    // Start is called before the first frame update
    void Start()
    {
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

        if (collision.gameObject.CompareTag("RedBloodCell"))
        {
            redBloodCellTransform = collision.transform;
            redBloodCellInJumpRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RedBloodCell"))
        {
            redBloodCellInJumpRange = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision: " + collision.gameObject.name);
    }
}
