using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusTrackTargetState : BaseState<A_VirusStateMachine.EState>
{
    private Unit pathFinder;
    private Movement movement;
    private Attack attack;
    private Health health;

    private LayerMask targetMask;
    private float detectionRadius;
    private Transform target;
    private bool jumpToTarget;
    private JumpTriggerBox jumpTriggerBox;

    public A_VirusTrackTargetState(A_VirusStateMachine.EState key, Unit pathFinder, Movement movement, LayerMask targetMask, JumpTriggerBox jumpTriggerBox, float detectionRadius, Attack attack, Health health) : base(key)
    {
        this.pathFinder = pathFinder;
        this.movement = movement;
        this.targetMask = targetMask;
        this.jumpTriggerBox = jumpTriggerBox;
        this.detectionRadius = detectionRadius;
        this.attack = attack;
        this.health = health;
    }

    public override void EnterState()
    {

        //Debug.Log("In Tracking State");
        jumpToTarget = false;

        movement.StopMovement();
        movement.ResetGravity();
        movement.ResetSpeed();

        if (!jumpTriggerBox.targetAcquired && target == null)
        {
            //Debug.Log("Null target on entry of tracking state");
            //Debug.Break();
            target = movement.GetTargetIfInRange(targetMask, detectionRadius, "RedBloodCell");
        }
    }

    public override void ExitState()
    {
        //movement.StopMovement();
        jumpToTarget = false;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        //target = movement.GetTargetIfInRange(targetMask, detectionRadius, "RedBloodCell");
        //pathFinder.SetConditions(target, true);
        if (jumpTriggerBox.targetAcquired && jumpTriggerBox.redBloodCellTransform == null)
        {
            jumpTriggerBox.targetAcquired = false;
        }

        if (movement.isGrounded())
        {
            //movement.MoveInHorizontalDirection(pathFinder.lookDir.normalized);
            movement.SetLookDirFacing(target.position);
            movement.MoveInHorizontalDirection();

            if (jumpTriggerBox.redBloodCellInJumpRange && movement.CanJump() && jumpTriggerBox.redBloodCellTransform != null)
            {
                movement.SetJumpForceBasedOnTarget(jumpTriggerBox.redBloodCellTransform);
                movement.JumpTowards();
            }

            else if (movement.FrontBlocked())
            {
                movement.JumpTowards(movement.jumpForce);
            }
        }
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        if (health.IsDead)
        {
            return A_VirusStateMachine.EState.Death;
        }
        else if (!movement.isGrounded())
        {
            return A_VirusStateMachine.EState.Jump;
        }
        else if (target != null)
        {
            return A_VirusStateMachine.EState.Track;
        }
        else
        {
            return A_VirusStateMachine.EState.Idle;
        }
    }

    #region Collision and Trigger logic
    public override void OnCollisionEnter2D(Collision2D other)
    {
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }

    #endregion

}
