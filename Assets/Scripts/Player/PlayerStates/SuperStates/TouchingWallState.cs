using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;
    public TouchingWallState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput=player.InputHandler.NormInputX;
        yInput=player.InputHandler.NormInputY;
        grabInput=player.InputHandler.grabInput;
        jumpInput=player.InputHandler.JumpInput;
        if(jumpInput)
        {
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            pSMachine.ChangeState(player.wallJumpState);
        }
        else if(isGrounded && !grabInput)
        {
            pSMachine.ChangeState(player.idleState);
        }
        else if(!isTouchingWall || (xInput!=player.facingDirection && !grabInput))
        {
            pSMachine.ChangeState(player.airState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
