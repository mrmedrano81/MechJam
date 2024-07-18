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
    A_VirusAnimationScript anim;

    public float detectionRadius;
    public LayerMask targetMask;
    public JumpTriggerBox jumpTriggerBox;

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

        anim = GetComponentInChildren<A_VirusAnimationScript>();

        states.Add(EState.Idle, new A_VirusIdleState(EState.Idle, pathFinder, movement, health));
        states.Add(EState.Jump, new A_VirusJumpState(EState.Jump, movement, attack, targetMask, health));
        states.Add(EState.Track, new A_VirusTrackTargetState(EState.Track, pathFinder, 
            movement, targetMask, jumpTriggerBox, detectionRadius, attack, health));
        states.Add(EState.Attack, new A_VirusAttackScript(EState.Attack, health));
        states.Add(EState.Death, new A_VirusDeathState(EState.Death, anim));

        currentState = states[EState.Idle];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
