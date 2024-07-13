using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_VirusAttackScript : BaseState<C_VirusStateMachine.EState>
{
    C_VirusSO c_virusSO;

    public C_VirusAttackScript(C_VirusStateMachine.EState key, C_VirusSO _c_virusSO) : base(key)
    {
        c_virusSO = _c_virusSO;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }
    public override void UpdateState()
    {
        Debug.Log("Attacking cell (kinda)");
    }

    public override void FixedUpdateState()
    {
    }

    public override C_VirusStateMachine.EState GetNextState()
    {
        return C_VirusStateMachine.EState.Attack;
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
