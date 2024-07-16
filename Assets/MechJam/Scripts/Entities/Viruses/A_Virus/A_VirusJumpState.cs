using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusJumpState : BaseState<A_VirusStateMachine.EState>
{
    private Movement movement;
    private Attack attack;
    private LayerMask targetMask;
    private Transform target;

    public A_VirusJumpState(A_VirusStateMachine.EState key, Movement movement, Attack attack, LayerMask targetMask) : base(key)
    {
        this.movement = movement;
        this.attack = attack;
        this.targetMask = targetMask;
    }

    public override void EnterState()
    {
        //if (movement.CheckRange(targetMask, attack.range, "RedBloodCell"))
        if (!movement.CheckRange(targetMask, attack.range, "RedBloodCell"))
        {
            Debug.Log("Enter: not in range");
            Debug.Break();
        }

        target = movement.GetTargetIfInRange(targetMask, attack.range, "RedBloodCell", true);

        if (target != null)
        {
            movement.SetJumpForceBasedOnTarget(target);

            if (movement.CanJump())
            {
                movement.JumpTowards();
            }
        }
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

    public override A_VirusStateMachine.EState GetNextState()
    {
        if (movement.isGrounded())
        {
            if (target == null)
            {
                Debug.Log("Null target");
                Debug.Break();
            }
            if (movement.CheckRange(targetMask, attack.range, "RedBloodCell") && target != null)
            {
                return A_VirusStateMachine.EState.Track;
            }
            else
            {
                return A_VirusStateMachine.EState.Idle;
            }
        }
        else
        {
            return A_VirusStateMachine.EState.Jump;
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
