using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : GroundedState
{
    public PlayerCrouchIdleState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.SetColliderHeight(pData.crouchColliderHeight);
    }
    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(pData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isExitingState)
        {
            if(inputX!=0)
            {
                pSMachine.ChangeState(player.crouchMoveState);
            }
            else if(inputY!=-1 && !isTouchingCeiling)
            {
                pSMachine.ChangeState(player.idleState);
            }
        }
    }

}
