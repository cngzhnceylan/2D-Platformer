using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : GroundedState
{
    public PlayerCrouchMoveState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
            player.SetVelocityX(pData.crouchMoveVelocity * player.facingDirection);
            player.CheckIfShouldFlip(inputX);
            if(inputX ==0)
            {
                pSMachine.ChangeState(player.crouchIdleState);
            }
            else if(inputY!=-1 && !isTouchingCeiling)
            {
                pSMachine.ChangeState(player.moveState);
            }
        }
    }

}
