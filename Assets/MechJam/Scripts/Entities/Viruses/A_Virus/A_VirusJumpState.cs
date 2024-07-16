using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusJumpState : BaseState<A_VirusStateMachine.EState>
{
    private Movement movement;

    public A_VirusJumpState(A_VirusStateMachine.EState key, Movement movement) : base(key)
    {
        this.movement = movement;
    }

    public override void EnterState()
    {
        movement.JumpTowards();
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
        if (movement.isGrounded())
        {
            return A_VirusStateMachine.EState.Idle;
        }
        else
        {
            return A_VirusStateMachine.EState.Jump;
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
