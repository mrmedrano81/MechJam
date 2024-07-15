using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAggroPlayerState : BaseState<WBCStateMachine.WBCState>
{
    Movement movementComponent;
    Unit pathFinder;

    Transform player;
    LayerMask playerMask;
    float maintainAgroRadius;
    bool playerInAggroRange = false;

    Transform virus;
    public LayerMask virusMask;
    public float detectVirusRadius;

    public WBCAggroPlayerState(
        WBCStateMachine.WBCState key,
        Unit _pathFinder,
        Movement _movementComponent,
        LayerMask _playerMask,
        float _maintainAggroRadius,
        LayerMask _virusMask,
        float _detectVirusRadius) 
        : base(key)
    {
        movementComponent = _movementComponent;
        pathFinder = _pathFinder;
        playerMask = _playerMask;
        maintainAgroRadius = _maintainAggroRadius;
        virusMask = _virusMask;
        detectVirusRadius = _detectVirusRadius;
    }

    public override void EnterState()
    {
        player = movementComponent.GetTargetIfInRange(playerMask, maintainAgroRadius);

        if (player != null)
        {
            playerInAggroRange = true;
            pathFinder.SetConditions(player, true);
            movementComponent.ResetSpeed();
        }
    }

    public override void ExitState()
    {
        pathFinder.SetConditions(null, false);
    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        if (movementComponent.CheckRange(virusMask, detectVirusRadius))
        {
            return WBCStateMachine.WBCState.AttackVirus;
        }
        else if (!movementComponent.CheckRange(playerMask, maintainAgroRadius))
        {
            Debug.Log("Not in aggro range");
            return WBCStateMachine.WBCState.Idle;
        }
        else
        {
            return WBCStateMachine.WBCState.AggroPlayer;
        }
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

    public override void UpdateState()
    {
        pathFinder.SetConditions(movementComponent.GetTargetIfInRange(playerMask, maintainAgroRadius), true);
    }

    public override void FixedUpdateState()
    {
        movementComponent.MoveTowardsDirection(pathFinder.lookDir);
    }
}
