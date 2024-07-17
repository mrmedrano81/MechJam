using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Unit))]
public class C_VirusStateMachine : StateManager<C_VirusStateMachine.EState>
{
    Movement movement;
    Health health;
    Attack attack;
    Unit pathFinder;

    public C_VirusSO c_virusSO;


    public enum EState
    {
        Moving,
        Attack,
        Death
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        attack = GetComponent<Attack>();
        pathFinder = GetComponent<Unit>();

        states.Add(EState.Moving, new C_VirusMovingState(EState.Moving, pathFinder, movement, c_virusSO));
        states.Add(EState.Attack, new C_VirusAttackScript(EState.Attack, c_virusSO, attack, movement));
        states.Add(EState.Death, new C_VirusDeathState(EState.Death));

        currentState = states[EState.Moving];
    }
}
