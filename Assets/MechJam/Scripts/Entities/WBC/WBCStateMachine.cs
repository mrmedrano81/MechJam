using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Movement))]
public class WBCStateMachine : StateManager<WBCStateMachine.WBCState>
{
    [Header("Player Aggro Parameters")]
    public LayerMask playerLayerMask;

    public float maintainAggroRadius;

    [Header("Virus Aggro Parameters")]
    public LayerMask virusLayerMask;
    public float detectVirusRadius;

    private Health healthComponent;
    private Attack attackComponent;
    private Movement movementComponent;

    private Unit WBCPathfinder;

    public enum WBCState
    {
        Idle,
        AggroPlayer,
        AggroVirus,
        AttackVirus
    }

    private void Awake()
    {

        healthComponent = GetComponent<Health>();
        attackComponent = GetComponent<Attack>();
        movementComponent = GetComponent<Movement>();

        WBCPathfinder = GetComponent<Unit>();

        states.Add(WBCState.Idle, 
            new WBCIdleState(
                WBCState.Idle, 
                movementComponent,
                playerLayerMask,
                maintainAggroRadius,
                virusLayerMask,
                detectVirusRadius));

        states.Add(WBCState.AggroPlayer, 
            new WBCAggroPlayerState(
                WBCState.AggroPlayer,
                WBCPathfinder,
                movementComponent, 
                playerLayerMask,
                maintainAggroRadius,
                virusLayerMask,
                detectVirusRadius
                ));

        states.Add(WBCState.AggroVirus,
            new WBCAggroVirusState(
                WBCState.AggroVirus,
                WBCPathfinder,
                movementComponent,
                virusLayerMask,
                detectVirusRadius
                ));

        states.Add(WBCState.AttackVirus, 
            new WBCAttackVirusState(
                WBCState.AttackVirus, 
                WBCPathfinder,
                movementComponent, 
                attackComponent,
                playerLayerMask,
                maintainAggroRadius,
                virusLayerMask,
                detectVirusRadius
                ));

        currentState = states[WBCState.Idle];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectVirusRadius);
    }
}
