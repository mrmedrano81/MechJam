using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Unit))]
public class A_VirusStateMachine : StateManager<A_VirusStateMachine.EState>
{
    Movement movement;
    Health health;
    Attack attack;
    Unit pathFinder;

    public float detectionRadius;
    public LayerMask targetMask;

    public enum EState
    {
        Idle,
        Jump,
        Track,
        Attack,
        Death
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        attack = GetComponent<Attack>();
        pathFinder = GetComponent<Unit>();

        states.Add(EState.Idle, new A_VirusIdleState(EState.Idle, pathFinder, movement));
        states.Add(EState.Jump, new A_VirusJumpState(EState.Jump, movement, attack, targetMask));
        states.Add(EState.Track, new A_VirusTrackTargetState(EState.Track, pathFinder, movement, targetMask, detectionRadius, attack));
        states.Add(EState.Attack, new A_VirusAttackScript(EState.Attack));
        states.Add(EState.Death, new A_VirusDeathState(EState.Death));

        currentState = states[EState.Idle];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
