using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_VirusIdleState : BaseState<A_VirusStateMachine.EState>
{
    Unit pathfInder;
    Movement movement;
    Health health;
    private float lastGroundChangeDirectionTime;

    private bool targetFound;

    public A_VirusIdleState(A_VirusStateMachine.EState key, Unit _pathFinder, Movement _g_Movement, Health health) : base(key)
    {
        pathfInder = _pathFinder;
        movement = _g_Movement;
        this.health = health;
    }

    public override void EnterState()
    {
        Debug.Log("Entering IdleState");
        movement.ResetSpeed();
        //movement.GetRandomGroundDirection();
        lastGroundChangeDirectionTime = Time.time;
        targetFound = false;
    }

    public override void ExitState()
    {
        movement.StopMovement();
    }

    public override void UpdateState()
    {
        //Debug.Log("In IdleState");
    }

    public override void FixedUpdateState()
    {
        if (Time.time - lastGroundChangeDirectionTime > movement.changeDirectionCooldown)
        {
            movement.GetRandomGroundDirection();
            lastGroundChangeDirectionTime = Time.time;
        }
        movement.GroundPatrol();

    }


    public override A_VirusStateMachine.EState GetNextState()
    {
        if (health.IsDead)
        {
            return A_VirusStateMachine.EState.Death;
        }
        else if (movement.isGrounded())
        {
            if (movement.FrontBlocked())
            {
                movement.JumpTowards(movement.jumpForce);
                return A_VirusStateMachine.EState.Jump;
            }
            else if (targetFound)
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
            return A_VirusStateMachine.EState.Idle;
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
        if (other.gameObject.CompareTag("RedBloodCell"))
        {
            //Debug.Log("RBC found");
            targetFound = true;
        }
        else if (other.gameObject.CompareTag("WhiteBloodCell"))
        {
            //Debug.Log("WBC found");
            targetFound = true;
        }

        //Debug.Log("targetFound: " + targetFound);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

    public override void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("RedBloodCell"))
        {
            targetFound = true;
        }
        else if (other.gameObject.CompareTag("WhiteBloodCell"))
        {
            targetFound = true;
        }
    }
    #endregion
}
