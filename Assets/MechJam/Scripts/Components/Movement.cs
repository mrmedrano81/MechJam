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

    [Header("Jump Settings")]
    public Vector2 jumpForce;
    public float f_jumpForce;
    public float jumpCooldown;
    private Vector2 currentJumpForce;
    private float lastJumpTime;
    public int baseGravity;
    public int fallSpeedMultiplier;
    public int maxFallSpeed;

    private Transform jumpTarget;
    public bool showJumpTarget = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        originalPosition = transform.position;
        patrolTarget = SetNewTarget();
        lookDir = (patrolTarget - (Vector2)transform.position);
        lastDirectionChangeTime = Time.time;
        currentJumpForce = jumpForce;
    }

    private void Start()
    {
        lastJumpTime = Time.time;
    }
    protected virtual void FixedUpdate()
    {
        if (balanceRB) BalanceRB();
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

    public void MoveInHorizontalDirection()
    {
       rb.velocity = new Vector2(lookDir.x * currentSpeed, rb.velocity.y);
    }

    public void GetRandomGroundDirection()
    {
        int randomFloat = UnityEngine.Random.Range(0, 20);

        if (randomFloat == 0)                           lookDir = new Vector2(0, 0);
        else if (randomFloat > 0 && randomFloat <= 12)   lookDir = new Vector2(-1, lookDir.y);
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

    #region Jumping
    public void SetJumpForceBasedOnTarget(Transform target)
    {
        jumpTarget = target;
        currentJumpForce = (target.position - transform.position);
        currentJumpForce = SetJumpForceToHitTarget(currentJumpForce);
    }

    public Vector2 SetJumpForceToHitTarget(Vector2 target)
    {
        //Debug.Log("Gravity scale Before VV calc: " + rb.gravityScale);
        float verticalVelocity = Mathf.Sqrt(Mathf.Abs(2 * rb.gravityScale * target.y));

        //Debug.Log("Vertical Velocity: " + verticalVelocity);

        float timeToPeak = verticalVelocity / rb.gravityScale;

        //Debug.Log("time to peak: " + timeToPeak);
        //Debug.Break();
        float horizontalVelocity = target.x / timeToPeak;

        //Debug.Log(verticalVelocity + ", " + timeToPeak + ", " + horizontalVelocity);

        Vector2 newJumpForce = new Vector2(horizontalVelocity, verticalVelocity)*f_jumpForce;

        return newJumpForce;
    }

    public void ResetJumpForce()
    {
        currentJumpForce = jumpForce;
    }

    public void JumpTowards(Vector2 _jumpForce)
    {
        if (CanJump())
        {
            rb.velocity = new Vector2(Mathf.Sign(lookDir.x) * jumpForce.x, _jumpForce.y);
            lastJumpTime = Time.time;
        }
    }

    public void JumpTowards()
    {
        if (CanJump())
        {
            rb.velocity = currentJumpForce;
            //Vector2 jumpDirection = currentJumpForce * f_jumpForce;
            //rb.AddForce(new Vector2(currentJumpForce.x * f_jumpForce, f_jumpForce), ForceMode2D.Impulse);
            //rb.velocity = jumpForce;
            //rb.velocity = new Vector2(currentJumpForce.x, currentJumpForce.y*f_jumpForce);
            lastJumpTime = Time.time;
        }
    }

    public bool CanJump()
    {
        if (Time.time - lastJumpTime > jumpCooldown && isGrounded())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetGravity()
    {
        rb.gravityScale = baseGravity;
    }

    public void ModifyGravityForFalling()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }

        else
        {
            rb.gravityScale = baseGravity;
        }
    }
    #endregion

    #region Direction and Position
    public void UpdateOriginalPosition()
    {
        originalPosition = transform.position;
    }

    public void UpdateOriginalPosition(Transform _newTransform)
    {
        originalPosition = _newTransform.position;
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

    public void BalanceRB()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetLookDir(Vector3 _lookDir)
    {
        lookDir = _lookDir;
    }

    public void SetLookDirFacing(Vector3 target)
    {
        lookDir = (target - transform.position).normalized;
    }

    #endregion

    #region Speed and General Movement
    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
        currentSpeed = 0;
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

    public void StickToTarget(Transform targetTransform, Vector3 randPoint)
    {

        transform.position = targetTransform.position + randPoint;
    }

    public Vector2 GetRandomStickOffset(float randRadius)
    {
        float offsetX = UnityEngine.Random.Range(0, randRadius);
        float offsetY = UnityEngine.Random.Range(0, randRadius);

        return new Vector3(offsetX, offsetY);
    }
    #endregion

    #region Range Functions
    public Transform GetTargetIfInRange(LayerMask _layerMask, float _searchRadius, string tag = "None", bool targetNearest = false)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _layerMask);

        Array.Reverse(hits);
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
                    else
                    {

                    }
                }

                return nearest;
            }

            else
            {
                //Debug.Log(hits.Length);
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
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius);

        //RaycastHit2D ray = Physics2D.ra

        Array.Reverse(hits);
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
    #endregion

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

        if (jumpTarget != null && showJumpTarget)
        {
            Gizmos.DrawCube(jumpTarget.transform.position, new Vector3(1, 1, 1));
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, patrolTarget);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, resetPathDistance);
    }
}
