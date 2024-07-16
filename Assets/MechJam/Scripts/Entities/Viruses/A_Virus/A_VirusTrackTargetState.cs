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

    public A_VirusTrackTargetState(
        A_VirusStateMachine.EState key, 
        Unit pathFinder, 
        Movement movement, 
        LayerMask targetMask,
        float detectionRadius,
        Attack attack
        ) : base(key)
    {
        this.pathFinder = pathFinder;
        this.movement = movement;
        this.targetMask = targetMask;
        this.detectionRadius = detectionRadius;
        this.attack = attack;
    }

    public override void EnterState()
    {
        jumpToTarget = false;
        target = movement.GetTargetIfInRange(targetMask, detectionRadius, "RedBloodCell");
        movement.ResetSpeed();

        if (target != null)
        {
            pathFinder.SetConditions(target, true);
        }
        else
        {
            Debug.Log("Null target");
            //Debug.Break();
        }
    }

    public override void ExitState()
    {
        //movement.StopMovement();
        jumpToTarget = false;
        target = null;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        pathFinder.SetConditions(target, true);

        if (movement.isGrounded())
        {
            movement.MoveInHorizontalDirection(pathFinder.lookDir);
        }
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        if (attack.DistanceFromTarget(target.position) <= attack.range && movement.CanJump())
        {
            return A_VirusStateMachine.EState.Jump;
        }
        else
        {
            return A_VirusStateMachine.EState.Track;
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
