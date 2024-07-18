using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_VirusMovingState : BaseState<C_VirusStateMachine.EState>
{

    Unit pathfInder;
    Movement movement;
    Health health;

    C_VirusSO c_virusSO;

    Transform target;
    bool transitionToAttack;

    public C_VirusMovingState(C_VirusStateMachine.EState key, Unit _pathFinder, Movement _movement, C_VirusSO _c_virusSO, Health health) : base(key)
    {
        transitionToAttack = false;
        pathfInder = _pathFinder;
        movement = _movement;
        c_virusSO = _c_virusSO;
        this.health = health;
    }

    public override void EnterState()
    {
        transitionToAttack = false;

        target = null;
        for (int j = 0; j < c_virusSO.targetTags.Length; j++)
        {
            target = movement.GetTargetIfInRange(c_virusSO.targetLayer, Mathf.Infinity, c_virusSO.targetTags[j], true);

            if (target != null)
            {
                return;
            }
            else
            {
                Debug.Log("No Target Found");
            }
        }
    }

    public override void ExitState()
    {
        transitionToAttack = false;
        pathfInder.SetConditions(null, false);
        //movement.StopMovement();
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        pathfInder.SetConditions(target, true);
        movement.MoveTowardsDirection(pathfInder.lookDir);
    }


    public override C_VirusStateMachine.EState GetNextState()
    {
        if (health.IsDead)
        {
            return C_VirusStateMachine.EState.Death;
        }
        else if (transitionToAttack == true)
        {
            return C_VirusStateMachine.EState.Attack;
        }
        else
        {
            return C_VirusStateMachine.EState.Moving;
        }
    }

    #region Collision and Trigger Logic
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
        foreach (string _tag in c_virusSO.targetTags)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                transitionToAttack = true;
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {

    }
    #endregion
}
