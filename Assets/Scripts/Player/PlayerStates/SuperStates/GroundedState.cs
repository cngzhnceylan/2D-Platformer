using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    protected int inputX;
    private bool JumpInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isTouchingWall;
    public GroundedState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded=player.CheckIfGrounded();
        isTouchingWall=player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpState.ResetAmountOfJumpsLeft();
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
        if(JumpInput && player.jumpState.CanJump())
        {
            pSMachine.ChangeState(player.jumpState);
        }
        else if(!isGrounded)
        {
            player.airState.StartCoyoteTime();
            pSMachine.ChangeState(player.airState);
        }
        else if(isTouchingWall&&grabInput)
        {
            pSMachine.ChangeState(player.wallGrabState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
