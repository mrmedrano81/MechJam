using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAggroPlayerState : BaseState<WBCStateMachine.WBCState>
{
    Movement movementComponent;
    Health health;
    Attack attack;
    Unit pathFinder;

    Transform player;
    private Vector2 playerStickOffset;
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
        float _detectVirusRadius,
        Health _health,
        Attack _attack) 
        : base(key)
    {
        movementComponent = _movementComponent;
        pathFinder = _pathFinder;
        playerMask = _playerMask;
        maintainAgroRadius = _maintainAggroRadius;
        virusMask = _virusMask;
        detectVirusRadius = _detectVirusRadius;
        health = _health;
        attack = _attack;
    }

    public override void EnterState()
    {
        player = movementComponent.GetTargetIfInRange(playerMask, maintainAgroRadius);
        playerStickOffset = movementComponent.GetRandomStickOffset(health.randStickRange);

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

    public override void UpdateState()
    {
        pathFinder.SetConditions(movementComponent.GetTargetIfInRange(playerMask, maintainAgroRadius), true);
    }

    public override void FixedUpdateState()
    {
        movementComponent.MoveTowardsDirection(pathFinder.lookDir);

        if (movementComponent.CheckRange(playerMask, attack.range, "Player"))
        {
            movementComponent.StickToTarget(player.transform, playerStickOffset);
        }

    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        if (health.IsDead)
        {
            return WBCStateMachine.WBCState.Death;
        }
        else if (movementComponent.CheckRange(virusMask, detectVirusRadius))
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
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
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


}
