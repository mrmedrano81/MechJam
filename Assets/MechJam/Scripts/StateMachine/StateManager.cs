using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> currentState;

    protected bool isTransitioningState = false;

    void Start()
    {
        currentState.EnterState();
    }

    void FixedUpdate()
    {
        EState nextStateKey = currentState.GetNextState();

        Debug.Log("State: " + nextStateKey);

        if (!isTransitioningState && nextStateKey.Equals(currentState.StateKey))
        {
            currentState.FixedUpdateState();
        } 
    }

    void Update()
    {
        EState nextStateKey = currentState.GetNextState();

        Debug.Log("State: " + nextStateKey);

        if (!isTransitioningState && nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    private void TransitionToState(EState nextStateKey)
    {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = states[nextStateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        currentState.OnTriggerStay2D(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        currentState.OnTriggerExit2D(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        currentState.OnCollisionStay2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentState.OnCollisionExit2D(collision);
    }
}
