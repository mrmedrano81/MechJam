using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class RedBCStateMachine : StateManager<RedBCStateMachine.RedBCEState>
{
    Movement movement;
    Health health;
    Animator anim;

    public RedBloodCellSO redBCSO;

    public enum RedBCEState
    {
        Idle,
        Death
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();

        states.Add(RedBCEState.Idle, new RedBCIdleState(RedBCEState.Idle, redBCSO, movement, health));
        states.Add(RedBCEState.Death, new RedBCDeathState(RedBCEState.Death, anim));
        currentState = states[RedBCEState.Idle];
    }
}
