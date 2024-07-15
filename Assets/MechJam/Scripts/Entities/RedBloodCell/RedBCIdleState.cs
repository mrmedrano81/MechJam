using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBCIdleState : BaseState<RedBCStateMachine.RedBCEState>
{
    private RedBloodCellSO redBCSO;
    private Movement movement;
    private Health health;

    public RedBCIdleState(RedBCStateMachine.RedBCEState key, RedBloodCellSO _redBCSO, Movement _movement, Health _health) : base(key)
    {
        redBCSO = _redBCSO;
        movement = _movement;
        health = _health;
    }

    public override void EnterState()
    {
        movement.UpdateOriginalPosition();
        movement.SetNewPatrolPath();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
        movement.Patrol();
    }

    public override RedBCStateMachine.RedBCEState GetNextState()
    {
        if (health.IsDead)
        {
            return RedBCStateMachine.RedBCEState.Death;
        }
        else
        {
            return RedBCStateMachine.RedBCEState.Idle;
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
