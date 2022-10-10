using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    protected int inputX;
    private bool JumpInput;
    private bool grabInput;
    private bool dashInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    public GroundedState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded=player.CheckIfGrounded();
        isTouchingWall=player.CheckIfTouchingWall();
        isTouchingLedge=player.CheckIfTouchingLedge();
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpState.ResetAmountOfJumpsLeft();
        player.dashState.ResetCanDash();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        inputX=player.InputHandler.NormInputX;
        JumpInput=player.InputHandler.JumpInput;
        grabInput=player.InputHandler.grabInput;
        dashInput=player.InputHandler.dashInput;
        if(JumpInput && player.jumpState.CanJump())
        {
            pSMachine.ChangeState(player.jumpState);
        }
        else if(!isGrounded)
        {
            player.airState.StartCoyoteTime();
            pSMachine.ChangeState(player.airState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            pSMachine.ChangeState(player.wallGrabState);
        }
          else if(dashInput && player.dashState.CheckIfCanDash())
        {
            pSMachine.ChangeState(player.dashState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
