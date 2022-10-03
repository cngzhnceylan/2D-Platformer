using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : GroundedState
{
    public PlayerMoveState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.CheckIfShouldFlip(inputX);
        player.SetVelocityX(pData.movementVelocity * inputX);
        if(inputX ==0)
        {
            pSMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
