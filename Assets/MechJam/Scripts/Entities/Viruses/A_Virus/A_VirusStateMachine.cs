using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundMovement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Unit))]
public class A_VirusStateMachine : StateManager<A_VirusStateMachine.EState>
{
    GroundMovement g_movement;
    Health health;
    Unit pathFinder;

    public enum EState
    {
        Idle,
        Attack,
        Death
    }

    private void Awake()
    {
        g_movement = GetComponent<GroundMovement>();
        health = GetComponent<Health>();
        pathFinder = GetComponent<Unit>();

        states.Add(EState.Idle, new A_VirusIdleState(EState.Idle, pathFinder, g_movement));
        states.Add(EState.Attack, new A_VirusAttackScript(EState.Attack));
        states.Add(EState.Death, new A_VirusDeathState(EState.Death));

        currentState = states[EState.Idle];
    }
}
