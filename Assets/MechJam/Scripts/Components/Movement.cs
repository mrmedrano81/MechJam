using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float slowingSpeed;
    public bool balanceRB;
    private float currentSpeed;
    private Vector2 currentLookDir;
    private Vector2 lookDir;
    private Rigidbody2D rb;

    [Header("Patrol Settings")]
    public float patrolMoveSpeedMultiplier;
    public float patrolRange;
    public float collisionDetectRadius;
    public float resetPathDistance;
    public LayerMask solidBlockMask;
    public float changeDirectionCooldown;
    public Vector3 originalPosition;
    private float lastDirectionChangeTime;
    private Vector2 patrolTarget;

    [Header("Ground Settings")]
    public Transform checkGround;
    public Transform checkForwardPath;
    public float forwardPathCheckDistance;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;
    public Vector2 jumpForce;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        originalPosition = transform.position;
        patrolTarget = SetNewTarget();
        lookDir = (patrolTarget - (Vector2)transform.position);
        lastDirectionChangeTime = Time.time;
    }

    protected virtual void FixedUpdate()
    {
        if (balanceRB) BalanceRB();
    }

    public void BalanceRB()
    {
        transform.rotation = Quaternion.identity;
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

    #region Patrol functions
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
        MoveTowardsDirection(lookDir, patrolMoveSpeedMultiplier);
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

    #endregion


    #region Ground Functions

    public bool isGrounded()
    {
        if (Physics2D.OverlapBox(checkGround.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    public void GroundPatrol()
    {
        MoveInHorizontalDirection(lookDir);
    }

    public void JumpTowards(Vector2 _jumpForce)
    {
        rb.velocity = _jumpForce;
    }

    public void JumpTowards()
    {
        rb.velocity = jumpForce;
    }

    public void MoveInHorizontalDirection(Vector2 lookDir, float _patrolMoveSpeedMult = 0)
    {
        if (_patrolMoveSpeedMult != 0)
        {
            rb.velocity = new Vector2(lookDir.x * currentSpeed * _patrolMoveSpeedMult, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(lookDir.x * currentSpeed, rb.velocity.y);
        }
    }

    public void GetRandomGroundDirection()
    {
        int randomFloat = UnityEngine.Random.Range(0, 11);

        if (randomFloat == 0)                           lookDir = new Vector2(0, 0);
        else if (randomFloat > 0 && randomFloat <= 5)   lookDir = new Vector2(-1, lookDir.y);
        else                                            lookDir = new Vector2(1, lookDir.y);
    }

    public bool FrontBlocked(Transform _eyeLevel, float _checkDistance, LayerMask _mask)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)_eyeLevel.position, new Vector2(lookDir.x,0), _checkDistance, _mask);

        if (hit) return true;
        else return false;
    }

    public bool FrontBlocked()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            (Vector2)checkForwardPath.position, 
            new Vector2(lookDir.x, 0), 
            forwardPathCheckDistance, solidBlockMask);

        if (hit) return true;
        else return false;
    }

    #endregion

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

    public void MoveTowardsDirection(Vector2 _lookDir, float _patrolMultiplier = 0)
    {
        if (_patrolMultiplier != 0)
        {
            rb.velocity = _lookDir.normalized * currentSpeed * _patrolMultiplier;
        }
        else
        {
            rb.velocity = _lookDir.normalized * currentSpeed;
        }
    }

    public void ChangeSpeedGradual(float _finalSpeed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, _finalSpeed, Time.deltaTime * slowingSpeed);
    }

    public Transform GetTargetIfInRange(LayerMask _layerMask, float _searchRadius, string tag = "None", bool targetNearest = false)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _layerMask);

        if (hits.Length > 0)
        {
            if (targetNearest == true)
            {
                Transform nearest = null;
                float shortestDistance = Mathf.Infinity;
                foreach (Collider2D hit in hits)
                {
                    if (tag == "None")
                    {
                        float currentDistance = Vector3.Distance(transform.position, hit.gameObject.transform.position);

                        if (shortestDistance > currentDistance)
                        {
                            shortestDistance = currentDistance;
                            nearest = hit.gameObject.transform;
                        }

                    }
                    else if (hit.gameObject.CompareTag(tag))
                    {
                        float currentDistance = Vector3.Distance(transform.position, hit.gameObject.transform.position);

                        if (shortestDistance > currentDistance)
                        {
                            shortestDistance = currentDistance;
                            nearest = hit.gameObject.transform;
                        }
                    }
                }

                return nearest;

            }
            else
            {
                Debug.Log(hits.Length);
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

        if (checkForwardPath != null)
        {
            Gizmos.DrawWireSphere(checkForwardPath.position, forwardPathCheckDistance);

        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, patrolTarget);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, resetPathDistance);
    }
}
