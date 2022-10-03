using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool coyoteTime;
    private bool isJumping;
    public PlayerInAirState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded=player.CheckIfGrounded();
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

        CheckCoyoteTime();
        xInput=player.InputHandler.NormInputX;
        jumpInput=player.InputHandler.JumpInput;
        jumpInputStop=player.InputHandler.JumpInputStop;
        CheckJumpMultiplier();
        if(isGrounded && player.currentVelocity.y< 0.01f)
        {
            pSMachine.ChangeState(player.landState);
        }
        else if(jumpInput && player.jumpState.CanJump())
        {
            pSMachine.ChangeState(player.jumpState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(pData.movementVelocity*xInput);
            
            player.anim.SetFloat("yVelocity",player.currentVelocity.y);
        }
    }
    private void CheckJumpMultiplier()
    {
        if(isJumping)
        {
            if(jumpInputStop)
            {
                player.SetVelocityY(player.currentVelocity.y * pData.jumpHeightMultiplier);
                isJumping=false;
            }
            else if(player.currentVelocity.y <=0 )
            {
                isJumping=false;
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    private void CheckCoyoteTime()
    {
        if(coyoteTime && Time.time > startTime + pData.coyoteTime)
        {
            coyoteTime=false;
            player.jumpState.DecreaseAmountOfJumpsLeft();
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping=true;

}
