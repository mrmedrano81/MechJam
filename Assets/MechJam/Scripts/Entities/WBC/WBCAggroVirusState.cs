using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAggroVirusState : BaseState<WBCStateMachine.WBCState>
{
    Movement movementComponent;
    Unit pathFinder;

    Transform virus;
    public LayerMask virusMask;
    public float detectVirusRadius;
    private bool transitionToAttack;
    private Health health;

    public WBCAggroVirusState(
        WBCStateMachine.WBCState key,
        Unit _pathFinder,
        Movement _movementComponent,
        LayerMask _virusMask,
        float _detectVirusRadius,
        Health health)
        : base(key)
    {
        movementComponent = _movementComponent;
        pathFinder = _pathFinder;
        virusMask = _virusMask;
        detectVirusRadius = _detectVirusRadius;
        this.health = health;
    }

    public override void EnterState()
    {
        transitionToAttack = false;
        virus = null;
        virus = movementComponent.GetTargetIfInRange(virusMask, detectVirusRadius);
    }

    public override void ExitState()
    {
        transitionToAttack = false;
        pathFinder.SetConditions(null, false);
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        //Debug.Log("fixedupdate");
        pathFinder.SetConditions(virus, true);
        movementComponent.MoveTowardsDirection(pathFinder.lookDir);
    }


    public override WBCStateMachine.WBCState GetNextState()
    {
        if (health.IsDead)
        {
            return WBCStateMachine.WBCState.Death;
        }
        else if (transitionToAttack == true)
        {
            return WBCStateMachine.WBCState.AttackVirus;
        }
        else if (virus != null)
        {
            return WBCStateMachine.WBCState.AggroVirus;
        }
        else
        {
            return WBCStateMachine.WBCState.Idle;
        }
    }

    #region Collision and Trigger logic
    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("A_Virus") || other.gameObject.CompareTag("C_Virus"))
        {
            transitionToAttack = true;
        }
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("A_Virus") || other.gameObject.CompareTag("C_Virus"))
        {
            transitionToAttack = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
    #endregion

}
