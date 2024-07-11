using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCScript : Unit
{
    [Header("WBC Parameters")]
    [SerializeField] private LayerMask layerMasks;

    [Header("Movement Parameters")]
    public float directionChangeSpeed;
    public float slowingSpeed;


    Vector3 currentDir;
    float currentSpeed;
    bool inRange;

    protected override void Awake()
    {
        base.Awake();
        inRange = false;
    }

    protected override void Start()
    {
        base.Start();
        currentDir = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();
        CheckRange();
        //MovementLogic();
    }

    public void CheckRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, searchRadius, (Vector2)transform.position, 0f, layerMasks);

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

    //public void MovementLogic()
    //{
    //    if (inRange)
    //    {
    //        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime * slowingSpeed);
    //    }
    //    else
    //    {
    //        // Gradually change the direction to lookDir
    //        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.fixedDeltaTime * slowingSpeed);
    //        //currentDir = Vector3.Lerp(currentDir, lookDir, Time.fixedDeltaTime * directionChangeSpeed);
    //        currentDir = lookDir;
    //        //rb.velocity = lookDir * speed * Time.fixedDeltaTime;
    //    }
    //    rb.velocity = currentDir.normalized * currentSpeed * Time.fixedDeltaTime;
    //}

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
