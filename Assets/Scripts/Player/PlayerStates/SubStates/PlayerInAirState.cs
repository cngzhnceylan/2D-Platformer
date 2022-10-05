using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool grabInput;
    private bool isGrounded;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;

    private float startWallJumpCoyoteTime;
    public PlayerInAirState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        oldIsTouchingWall=isTouchingWall;
        oldIsTouchingWallBack=isTouchingWallBack;

        isGrounded=player.CheckIfGrounded();
        isTouchingWall=player.CheckIfTouchingWall();
        isTouchingWallBack=player.CheckIfTouchingWallBack();

        if(!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Exit()
    {
        base.Exit();
        oldIsTouchingWall=false;
        oldIsTouchingWallBack=false;
        isTouchingWall=false;
        isTouchingWallBack=false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        xInput=player.InputHandler.NormInputX;
        jumpInput=player.InputHandler.JumpInput;
        jumpInputStop=player.InputHandler.JumpInputStop;
        grabInput=player.InputHandler.grabInput;
        CheckJumpMultiplier();
        if(isGrounded && player.currentVelocity.y< 0.01f)
        {
            pSMachine.ChangeState(player.landState);
        }
        else if(jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall=player.CheckIfTouchingWall();
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            pSMachine.ChangeState(player.wallJumpState);
        }
        else if(jumpInput && player.jumpState.CanJump())
        {
            pSMachine.ChangeState(player.jumpState);
        }
        else if(isTouchingWall && grabInput)
        {
            pSMachine.ChangeState(player.wallGrabState);
        }
        else if(isTouchingWall && xInput==player.facingDirection && player.currentVelocity.y<=0)
        {
            pSMachine.ChangeState(player.wallSlideState);
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
    private void CheckWallJumpCoyoteTime()
    {
        if(wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + pData.coyoteTime)
        {
            wallJumpCoyoteTime=false;
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime=true;
        startWallJumpCoyoteTime=Time.time;
    }
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime=false;
    public void SetIsJumping() => isJumping=true;

}
