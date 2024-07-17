using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusTrackTargetState : BaseState<A_VirusStateMachine.EState>
{
    private Unit pathFinder;
    private Movement movement;
    private Attack attack;

    private LayerMask targetMask;
    private float detectionRadius;
    private Transform target;
    private bool jumpToTarget;
    private JumpTriggerBox jumpTriggerBox;

    public A_VirusTrackTargetState(A_VirusStateMachine.EState key, Unit pathFinder, Movement movement, LayerMask targetMask, JumpTriggerBox jumpTriggerBox, float detectionRadius, Attack attack) : base(key)
    {
        this.pathFinder = pathFinder;
        this.movement = movement;
        this.targetMask = targetMask;
        this.jumpTriggerBox = jumpTriggerBox;
        this.detectionRadius = detectionRadius;
        this.attack = attack;
    }

    public override void EnterState()
    {

        Debug.Log("In Tracking State");
        jumpToTarget = false;

        movement.ResetGravity();
        movement.ResetSpeed();

        if (target != null)
        {
            //pathFinder.SetConditions(target, true);
        }
        else
        {
            Debug.Log("Null target on entry of tracking state");
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

        if (movement.isGrounded())
        {
            //movement.MoveInHorizontalDirection(pathFinder.lookDir.normalized);
            movement.SetLookDirFacing(target.position);
            movement.MoveInHorizontalDirection();


            if (jumpTriggerBox.redBloodCellInJumpRange && movement.CanJump())
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
        if (!movement.isGrounded())
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
