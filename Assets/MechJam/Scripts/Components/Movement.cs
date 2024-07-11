using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float slowingSpeed;
    public float rotationSpeed;
    private float currentRotationSpeed;
    private float currentSpeed;
    private Vector2 currentLookDir;
    private Vector2 lookDir;
    private Rigidbody2D rb;

    [Header("Patrol Settings")]
    public float patrolMoveSpeedScale;
    public float patrolRange;
    public float resetPathRadius;
    public float resetPathDistance;
    public LayerMask triggerPathChangeMask;
    public float changeDirectionCooldown;
    private float lastDirectionChangeTime;
    private Vector3 originalPosition;
    private Vector2 patrolTarget;


    private void Awake()
    {
        currentRotationSpeed = rotationSpeed;
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

    public void Patrol()
    {
        if (Time.time - lastDirectionChangeTime > changeDirectionCooldown)
        {
            //if (CheckForwardRange(triggerPathChangeMask, lookDir, resetPathDistance))
            //{
            //    ChangeDirection();
            //}

            if (CheckRange(triggerPathChangeMask, resetPathRadius))
            {
                ChangeDirection();
            }

            else if (Vector2.Distance((Vector2)transform.position, patrolTarget) < 0.1f)
            {
                Debug.Log("set point");
                StopMovement();
                ChangeSpeedGradual(moveSpeed);
                patrolTarget = SetNewTarget();
                lookDir = (patrolTarget - (Vector2)transform.position);
            }

            ChangeSpeedGradual(moveSpeed);
            MoveTowardsDirection(lookDir);
        }
    }


    private Vector2 SetNewTarget()
    {
        Vector2 randomExtension = new Vector2(Random.Range(-patrolRange, patrolRange), Random.Range(-patrolRange, patrolRange));
        Vector2 randomPoint = (Vector2)originalPosition + randomExtension;

        return randomPoint;
    }

    private Vector2 SetOppositeDirection(Vector2 direction)
    {
        return -direction;
    }

    private void ChangeDirection()
    {
        StopMovement();
        lookDir = SetOppositeDirection(lookDir);
        patrolTarget = (Vector2)transform.position + lookDir.normalized * resetPathDistance;
        lastDirectionChangeTime = Time.time;
    }

    public void MoveTowardsDirection(Vector2 _lookDir)
    {
        rb.velocity = _lookDir.normalized * moveSpeed * Time.deltaTime;
    }

    public void ChangeSpeedGradual(float _finalSpeed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, _finalSpeed, Time.deltaTime * slowingSpeed);
    }

    public bool CheckForwardRange(LayerMask _layerMask, Vector2 _searchDirection, float _searchLength, string tag = "None")
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _searchDirection, _searchLength, _layerMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (tag == "None")
                {
                    return true;
                }
                else if (hit.collider.gameObject.CompareTag(tag))
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

    public bool CheckRange(LayerMask _layerMask, float _searchRadius, string tag = "None")
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _searchRadius, (Vector2)transform.position, 0f, _layerMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (tag == "None")
                {
                    return true;
                }
                else if (hit.collider.gameObject.CompareTag(tag))
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
            Gizmos.DrawWireSphere(originalPosition, patrolRange);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, resetPathRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, patrolTarget);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, resetPathDistance);
    }
}
