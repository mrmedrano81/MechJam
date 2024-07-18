using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_VirusJumpState : BaseState<A_VirusStateMachine.EState>
{
    private Movement movement;
    private Attack attack;
    private Health health;
    private LayerMask targetMask;
    private Transform target;

    public A_VirusJumpState(A_VirusStateMachine.EState key, Movement movement, Attack attack, LayerMask targetMask, Health health) : base(key)
    {
        this.movement = movement;
        this.attack = attack;
        this.targetMask = targetMask;
        this.health = health;
    }

    public override void EnterState()
    {
        //if (movement.CheckRange(targetMask, attack.range, "RedBloodCell"))
        //if (!movement.CheckRange(targetMask, attack.range, "RedBloodCell"))
        //{
        //    Debug.Log("Enter: not in range");
        //}

        //target = movement.GetTargetIfInRange(targetMask, attack.range, "RedBloodCell", true);

        //if (target != null)
        //{
        //    movement.SetJumpForceBasedOnTarget(target);

        //    if (movement.CanJump())
        //    {
        //        movement.JumpTowards();
        //    }
        //}
        //else
        //{
        //    movement.JumpTowards(movement.jumpForce);
        //}

        
    }

    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
        movement.ModifyGravityForFalling();
    }

    public override A_VirusStateMachine.EState GetNextState()
    {
        if (health.IsDead)
        {
            return A_VirusStateMachine.EState.Death;
        }
        else if (movement.CanJump())
        {
            if (movement.CheckRange(targetMask, attack.range, "RedBloodCell"))
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
        if (other.gameObject.CompareTag("RedBloodCell"))
        {
            Health redBloodCellHealth = other.gameObject.GetComponent<Health>();
            redBloodCellHealth.TakeDamage(attack.damage);

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
