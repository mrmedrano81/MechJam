using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCIdleState : BaseState<WBCStateMachine.WBCState>
{
    private Movement movementComponent;

    public WBCIdleState(WBCStateMachine.WBCState key, Movement _movementComponent) : base(key)
    {
        movementComponent = _movementComponent;
    }

    public override void EnterState()
    {
        Debug.Log("WBCIdleState: Enter State");
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        //Debug.Log("WBCIdleState: Enter State " + StateKey);
        return StateKey;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collided with: " + other.gameObject.name);
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        movementComponent.Patrol();
        //Debug.Log("WBCIdleState: Update State");
    }
}
