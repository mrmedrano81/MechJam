using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCScript : Unit
{
    [Header("WBC Parameters")]
    [SerializeField] private LayerMask layerMasks;

    public Vector3 currentDir;
    public float directionChangeSpeed;

    protected override void Start()
    {
        base.Start();
        currentDir = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();
        CheckRange();
        MovementLogic();
    }

    public void CheckRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, searchRadius, (Vector2)transform.position, 0f, layerMasks);

        //Logger.Log(hits.Length);
        if (hits.Length < 1)
        {
            inRange = false;
            return;
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                inRange = true;
            }
        }
    }

    public void MovementLogic()
    {
        if (inRange)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            
            // Gradually change the direction to lookDir
            currentDir = Vector3.Slerp(currentDir, lookDir, Time.deltaTime * directionChangeSpeed);
            rb.velocity = currentDir.normalized * speed;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
