using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAttackVirusState : BaseState<WBCStateMachine.WBCState>
{
    private Attack attackComponent;
    private Movement movementComponent;

    public WBCAttackVirusState(WBCStateMachine.WBCState key, Movement _movementComponent, Attack _attackComponent) : base(key)
    {
        movementComponent = _movementComponent;
        attackComponent = _attackComponent;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        throw new System.NotImplementedException();
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
        Debug.Log("Damage: " + attackComponent.damage);
    }
}
