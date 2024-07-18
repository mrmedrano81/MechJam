using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCAttackVirusState : BaseState<WBCStateMachine.WBCState>
{
    private Attack attackComponent;
    private Movement movementComponent;
    private Health healthComponent;
    private Unit pathFinder;

    private Health virusHealth;
    LayerMask playerMask;
    float maintainAggroRadius;
    LayerMask virusMasks;
    float detectVirusRadius;
    bool virusGone;
    Vector2 virusStickOffset;

    public WBCAttackVirusState(
        WBCStateMachine.WBCState key,
        Unit _pathFinder,
        Movement _movementComponent, 
        Attack _attackComponent,
        LayerMask _playerMask,
        float _maintainAggroRadius,
        LayerMask _virusMasks,
        float _detectVirusRadius,
        Health healthComponent) : base(key)
    {
        pathFinder = _pathFinder;
        movementComponent = _movementComponent;
        attackComponent = _attackComponent;
        playerMask = _playerMask;
        virusMasks = _virusMasks;
        maintainAggroRadius = _maintainAggroRadius;
        detectVirusRadius = _detectVirusRadius;
        this.healthComponent = healthComponent;
    }

    public override void EnterState()
    {
        Debug.Log("Enter attack state WBC");
        virusGone = false;
        virusHealth = attackComponent.GetHealthComponentIfInRange(virusMasks, attackComponent.range);
        virusStickOffset = movementComponent.GetRandomStickOffset(virusHealth.randStickRange);
    }

    public override void ExitState()
    {
        pathFinder.SetConditions(null, false);
        virusHealth = null;
    }

    

    public override WBCStateMachine.WBCState GetNextState()
    {
        if (healthComponent.IsDead)
        {
            return WBCStateMachine.WBCState.Death;
        }
        else if (virusGone)
        {
            return WBCStateMachine.WBCState.Idle;
        }
        else
        {
            return WBCStateMachine.WBCState.AttackVirus;
        }
    }

    public override void UpdateState()
    {
        if (movementComponent.CheckRange(virusMasks, attackComponent.range))
        {
            //Debug.Log("Should stick");
            
            if (virusHealth != null)
            {
                movementComponent.StickToTarget(virusHealth.gameObject.transform, virusStickOffset);
                attackComponent.DoDamage(virusHealth);
                Attack virusAttack = virusHealth.gameObject.GetComponent<Attack>();
                if (virusAttack != null)
                {
                    virusAttack.DoDamage(healthComponent);
                }

            }
            else
            {
                virusGone = true;
            }
        }
        else
        {
            virusGone = true;
        }

        //Debug.Log("Damage: " + attackComponent.damage);
    }

    public override void FixedUpdateState()
    {

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
