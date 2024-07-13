using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_VirusIdleState : BaseState<A_VirusStateMachine.EState>
{
    Unit pathfInder;
    GroundMovement g_movement;

    public A_VirusIdleState(A_VirusStateMachine.EState key, Unit _pathFinder, GroundMovement _g_Movement) : base(key)
    {
        pathfInder = _pathFinder;
        g_movement = _g_Movement;
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

    }


    public override A_VirusStateMachine.EState GetNextState()
    {
        return A_VirusStateMachine.EState.Idle;
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
