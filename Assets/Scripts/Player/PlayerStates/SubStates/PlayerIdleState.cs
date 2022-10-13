using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : GroundedState
{
    public PlayerIdleState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(!isExitingState)
        {

            if(inputX != 0)
            {
                pSMachine.ChangeState(player.moveState);
            }
            else if(inputY == -1)
            {
                pSMachine.ChangeState(player.crouchIdleState);
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
