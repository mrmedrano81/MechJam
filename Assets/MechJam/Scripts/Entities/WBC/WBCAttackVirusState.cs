using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAttackVirusState : BaseState<WBCStateMachine.WBCState>
{
    private Attack attackComponent;
    private Movement movementComponent;
    private Unit pathFinder;

    private Transform virus;
    public LayerMask virusMask;
    public float detectVirusRadius;

    public WBCAttackVirusState(WBCStateMachine.WBCState key, Movement _movementComponent, Attack _attackComponent) : base(key)
    {
        movementComponent = _movementComponent;
        attackComponent = _attackComponent;
    }

    public override void EnterState()
    {
        pathFinder.SetConditions(virus, true);
    }

    public override void ExitState()
    {
        pathFinder.SetConditions(null, false);
    }

    

    public override WBCStateMachine.WBCState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        //Debug.Log("Damage: " + attackComponent.damage);
    }

    public override void FixedUpdateState()
    {
        //throw new System.NotImplementedException();
    }

    #region Collision and Trigger Logic
    public override void OnCollisionEnter2D(Collision2D other)
    {
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    #endregion



}
