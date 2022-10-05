using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : AbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        player.jumpState.ResetAmountOfJumpsLeft();
        player.SetVelocity(pData.wallJumpVelocity,pData.wallJumpAngle,wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.jumpState.DecreaseAmountOfJumpsLeft();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.anim.SetFloat("yVelocity",player.currentVelocity.y);

        if(Time.time >= startTime + pData.wallJumpTime)
        {
            isAbilityDone=true;
        }
    }
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if(isTouchingWall)
        {
            wallJumpDirection=-player.facingDirection;
        }
        else
        {
            wallJumpDirection=player.facingDirection;
        }
    }

}
