using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_VirusDeathState : BaseState<C_VirusStateMachine.EState>
{
    private C_VirusAnimationScript anim;

    public C_VirusDeathState(C_VirusStateMachine.EState key, C_VirusAnimationScript anim) : base(key)
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

    public override C_VirusStateMachine.EState GetNextState()
    {
        return C_VirusStateMachine.EState.Death;
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
