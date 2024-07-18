using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCIdleState : BaseState<WBCStateMachine.WBCState>
{
    private Movement movementComponent;
    private Health health;

    [Header("Player Aggro Parameters")]
    public Transform player;
    public LayerMask playerMask;
    public float maintainAggroRadius;

    [Header("Player Aggro Parameters")]
    public LayerMask virusMask;
    public float detectVirusRadius;

    public WBCIdleState(
        WBCStateMachine.WBCState key, 
        Movement _movementComponent,
        LayerMask _playerMask,
        float _maintainAggroRadius,
        LayerMask _virusMasks,
        float _detectVirusRadius
,
        Health health
        ) : base(key)
    {
        movementComponent = _movementComponent;
        playerMask = _playerMask;
        virusMask = _virusMasks;
        maintainAggroRadius = _maintainAggroRadius;
        detectVirusRadius = _detectVirusRadius;
        this.health = health;
    }

    public override void EnterState()
    {
        movementComponent.UpdateOriginalPosition();
        movementComponent.SetNewPatrolPath();
    }

    public override void ExitState()
    {
        movementComponent.StopMovement();
        movementComponent.ResetSpeed();
    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        if (health.IsDead)
        {
            return WBCStateMachine.WBCState.Death;
        }
        else if (movementComponent.CheckRange(virusMask, detectVirusRadius))
        {
            return WBCStateMachine.WBCState.AggroVirus;
        }
        else if (movementComponent.CheckRange(playerMask, maintainAggroRadius))
        {
            return WBCStateMachine.WBCState.AggroPlayer;
        }
        else
        {
            return WBCStateMachine.WBCState.Idle;
        }
    }

    #region Collision and Trigger logic

    public override void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("collided with: " + other.gameObject.name);
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
        
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        movementComponent.SetNewPatrolPath();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        
    }

    #endregion

    public override void UpdateState()
    {
        
        //Debug.Log("WBCIdleState: Update State");
    }

    public override void FixedUpdateState()
    {
        movementComponent.Patrol();
    }
}
