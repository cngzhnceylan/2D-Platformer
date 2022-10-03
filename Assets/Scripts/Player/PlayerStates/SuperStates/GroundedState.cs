using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    protected int inputX;
    private bool JumpInput;
    public bool isGrounded;
    public GroundedState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
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
        if(JumpInput && player.jumpState.CanJump())
        {
            player.InputHandler.UseJumpInput();
            pSMachine.ChangeState(player.jumpState);
        }
        else if(!isGrounded)
        {
            player.airState.StartCoyoteTime();
            pSMachine.ChangeState(player.airState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
