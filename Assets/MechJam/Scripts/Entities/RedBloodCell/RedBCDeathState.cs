using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBCDeathState : BaseState<RedBCStateMachine.RedBCEState>
{
    Animator anim;

    public RedBCDeathState(RedBCStateMachine.RedBCEState key, Animator _anim) : base(key)
    {
        anim = _anim;
    }

    public override void EnterState()
    {
        Debug.Log("Dead");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override RedBCStateMachine.RedBCEState GetNextState()
    {
        return StateKey;
    }

    #region Collision and Trigger logic
    public override void OnCollisionEnter2D(Collision2D other)
    {
    }

    public override void OnCollisionExit2D(Collision2D other)
    {
    }

    public override void OnCollisionStay2D(Collision2D other)
    {
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
    #endregion
}
