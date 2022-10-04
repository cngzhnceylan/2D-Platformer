using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : TouchingWallState
{
    private Vector2 holdPosition;
    public PlayerWallGrabState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
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
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition=player.transform.position;
        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        HoldPosition();

        if(yInput > 0)
        {
            pSMachine.ChangeState(player.wallClimbState);
        }
        else if(yInput < 0 || !grabInput)
        {
            pSMachine.ChangeState(player.wallSlideState);
        }
    }
    private void HoldPosition()
    {
        player.transform.position=holdPosition;
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
