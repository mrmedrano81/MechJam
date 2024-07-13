using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_VirusAttackScript : BaseState<C_VirusStateMachine.EState>
{
    C_VirusSO c_virusSO;
    Attack attack;
    Movement movement;
    Health cellHealth;

    bool targetDead;
    bool targetMovedAway;

    public C_VirusAttackScript(C_VirusStateMachine.EState key, C_VirusSO _c_virusSO, Attack _attack, Movement _movement) : base(key)
    {
        c_virusSO = _c_virusSO;
        attack = _attack;
        movement = _movement;
    }

    public override void EnterState()
    {
        targetDead = false;
        targetMovedAway = false;
    }

    public override void ExitState()
    {
        targetDead = false;
        targetMovedAway = false;
    }

    public override void UpdateState()
    {

        foreach (LayerMask layer in c_virusSO.targetLayers)
        {
            foreach (string tag in c_virusSO.targetTags)
            {
                //Debug.Log(layer + ", " + tag);

                if (movement.CheckRange(layer, attack.range, tag))
                {
                    cellHealth = attack.GetHealthComponentIfInRange(layer, attack.range, tag);
                    if (cellHealth != null)
                    {
                        if (cellHealth.IsDead)
                        {
                            targetDead = true;
                        }
                        else
                        {
                            attack.DoDamage(cellHealth);
                        }
                    }
                }
                else
                {
                    targetMovedAway = true;
                }
            }
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override C_VirusStateMachine.EState GetNextState()
    {
        if (targetDead || targetMovedAway)
        {
            return C_VirusStateMachine.EState.Moving;
        }
        else
        {
            return C_VirusStateMachine.EState.Attack;
        }
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
