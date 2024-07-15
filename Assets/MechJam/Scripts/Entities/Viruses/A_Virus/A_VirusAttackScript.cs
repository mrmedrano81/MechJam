using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusAttackScript : BaseState<A_VirusStateMachine.EState>
{
    public A_VirusAttackScript(A_VirusStateMachine.EState key) : base(key)
    {
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        throw new System.NotImplementedException();
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

    public override void UpdateState()
    {
    }
    #endregion
}
