using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_VirusIdleState : BaseState<A_VirusStateMachine.EState>
{
    Unit pathfInder;
    Movement movement;
    private float lastGroundChangeDirectionTime;

    public A_VirusIdleState(A_VirusStateMachine.EState key, Unit _pathFinder, Movement _g_Movement) : base(key)
    {
        pathfInder = _pathFinder;
        movement = _g_Movement;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        if (Time.time - lastGroundChangeDirectionTime > movement.changeDirectionCooldown)
        {
            movement.GetRandomGroundDirection();
            lastGroundChangeDirectionTime = Time.time;
        }
        movement.GroundPatrol();
    }


    public override A_VirusStateMachine.EState GetNextState()
    {
        if (movement.isGrounded() && movement.FrontBlocked())
        {
            return A_VirusStateMachine.EState.Jump;
        }
        else
        {
            return A_VirusStateMachine.EState.Idle;
        }
    }

    #region Collision and Trigger Logic
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
