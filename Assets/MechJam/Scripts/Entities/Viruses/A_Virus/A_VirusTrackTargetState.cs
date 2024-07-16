using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusTrackTargetState : BaseState<A_VirusStateMachine.EState>
{
    private Unit pathFinder;
    private Movement movement;

    private LayerMask targetMask;
    private float detectionRadius;
    private Transform target;

    public A_VirusTrackTargetState(
        A_VirusStateMachine.EState key, 
        Unit pathFinder, 
        Movement movement, 
        LayerMask targetMask,
        float detectionRadius
        ) : base(key)
    {
        this.pathFinder = pathFinder;
        this.movement = movement;
        this.targetMask = targetMask;
        this.detectionRadius = detectionRadius;
    }

    public override void EnterState()
    {
        target = movement.GetTargetIfInRange(targetMask, detectionRadius);
    }

    public override void ExitState()
    {
        target = null;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        if (target == null)
        {
            return A_VirusStateMachine.EState.Idle;
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
