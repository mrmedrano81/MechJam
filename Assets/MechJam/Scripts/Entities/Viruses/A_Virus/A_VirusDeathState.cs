using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusDeathState : BaseState<A_VirusStateMachine.EState>
{
    A_VirusAnimationScript anim;

    public A_VirusDeathState(A_VirusStateMachine.EState key, A_VirusAnimationScript anim) : base(key)
    {
        this.anim = anim;
    }

    public override void EnterState()
    {
        anim.playDeathAnim();
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        return A_VirusStateMachine.EState.Death;
    }

    #region Collider and Trigger logic
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
