using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float slowingSpeed;
    private float currentSpeed;
    private Vector2 currentLookDir;
    private Vector2 lookDir;
    private Rigidbody2D rb;

    [Header("Patrol Settings")]
    public float patrolMoveSpeed;
    public float patrolRange;
    public float collisionDetectRadius;
    public float resetPathDistance;
    public LayerMask solidBlockMask;
    public float changeDirectionCooldown;
    public Vector3 originalPosition;
    private float lastDirectionChangeTime;
    private Vector2 patrolTarget;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        originalPosition = transform.position;
        patrolTarget = SetNewTarget();
        lookDir = (patrolTarget - (Vector2)transform.position);
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
        currentSpeed = 0;
    }

    public void UpdateOriginalPosition()
    {
        originalPosition = transform.position;
    }

    public void UpdateOriginalPosition(Transform _newTransform)
    {
        originalPosition = _newTransform.position;
    }

    public void Patrol()
    {
        if (Time.time - lastDirectionChangeTime > changeDirectionCooldown)
        {
            if (CheckRange(solidBlockMask, collisionDetectRadius))
            {
                ChangeDirection();
            }

            else if (Vector2.Distance((Vector2)transform.position, patrolTarget) < 0.5f)
            {
                //Debug.Log("set point");
                StopMovement();
                patrolTarget = SetNewTarget();
                lookDir = (patrolTarget - (Vector2)transform.position);
            }
        }

        ChangeSpeedGradual(moveSpeed);
        MoveTowardsDirection(lookDir);
    }


    public void SetNewPatrolPath()
    {
        StopMovement();
        patrolTarget = SetNewTarget();
        lookDir = (patrolTarget - (Vector2)transform.position);
    }

    public Vector2 SetNewTarget()
    {
        Vector2 randomExtension = new Vector2(
            UnityEngine.Random.Range(-patrolRange, patrolRange), 
            UnityEngine.Random.Range(-patrolRange, patrolRange))/2;
        Vector2 randomPoint = (Vector2)originalPosition + randomExtension;

        return randomPoint;
    }

    public Vector2 SetNewTarget(float patrolRange)
    {
        Vector2 randomExtension = new Vector2(
            UnityEngine.Random.Range(-patrolRange, patrolRange),  
            UnityEngine.Random.Range(-patrolRange, patrolRange)) / 2;
        Vector2 randomPoint = (Vector2)originalPosition + randomExtension;

        return randomPoint;
    }

    public Vector2 SetOppositeDirection(Vector2 direction)
    {
        return -direction;
    }

    public void ChangeDirection()
    {
        StopMovement();
        lookDir = SetOppositeDirection(lookDir);
        patrolTarget = (Vector2)transform.position + lookDir.normalized * resetPathDistance;
        lastDirectionChangeTime = Time.time;
    }

    public void ResetSpeed()
    {
        currentSpeed = moveSpeed;
    }

    public void MoveTowardsDirection(Vector2 _lookDir)
    {
        rb.velocity = _lookDir.normalized * currentSpeed;
    }

    public void ChangeSpeedGradual(float _finalSpeed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, _finalSpeed, Time.deltaTime * slowingSpeed);
    }

    public Transform GetTargetIfInRange(LayerMask _layerMask, float _searchRadius, string tag = "None")
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _layerMask);

        Array.Reverse(hits);
        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (tag == "None")
                {
                    return hit.gameObject.transform;
                }
                else if (hit.gameObject.CompareTag(tag))
                {
                    return hit.gameObject.transform;
                }

                else return null;
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    public bool CheckRange(LayerMask _layerMask, float _searchRadius, string tag = "None")
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _layerMask);

        //RaycastHit2D ray = Physics2D.ra

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                

                if (tag == "None")
                {
                    return true;
                }
                else if (hit.gameObject.CompareTag(tag))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (originalPosition != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(originalPosition, new Vector3(patrolRange, patrolRange, 0));
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisionDetectRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, patrolTarget);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, resetPathDistance);
    }
}
