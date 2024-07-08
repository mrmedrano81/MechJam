using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCScript : Unit
{
    [Header("WBC Parameters")]
    [SerializeField] private LayerMask layerMasks;

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
            Logger.Log("Hits: ", hit);
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
            rb.velocity = lookDir * speed;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
