using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCDeathState : BaseState<WBCStateMachine.WBCState>
{
    WBCAnimationScript anim;
    public WBCDeathState(WBCStateMachine.WBCState key, WBCAnimationScript anim) : base(key)
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


    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override WBCStateMachine.WBCState GetNextState()
    {
        return WBCStateMachine.WBCState.Death;
    }

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

}
