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
    public Transform player;
    public LayerMask playerMask;
    public float maintainAggroRadius;

    [Header("Virus Aggro Parameters")]
    public LayerMask virusMask;
    public float detectVirusRadius;

    private Health healthComponent;
    private Attack attackComponent;
    private Movement movementComponent;

    private Unit WBCPathfinder;

    public enum WBCState
    {
        Idle,
        AggroPlayer,
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
                playerMask,
                maintainAggroRadius,
                virusMask,
                detectVirusRadius));

        states.Add(WBCState.AggroPlayer, 
            new WBCAggroPlayerState(
                WBCState.AggroPlayer,
                WBCPathfinder,
                movementComponent, 
                playerMask,
                maintainAggroRadius,
                virusMask,
                detectVirusRadius
                ));

        states.Add(WBCState.AttackVirus, 
            new WBCAttackVirusState(
                WBCState.AttackVirus, 
                movementComponent, 
                attackComponent
                ));

        currentState = states[WBCState.Idle];
    }
}
