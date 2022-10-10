using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPos;
    private Vector2 stopPos;
    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private int xInput;
    private int yInput;
    public PlayerLedgeClimbState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.anim.SetBool("ClimbLedge",false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging=true;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position=detectedPosition;
        cornerPosition=player.DetermineCornerPosition();

        startPos.Set(cornerPosition.x - (player.facingDirection * pData.startOffset.x),cornerPosition.y - pData.startOffset.y);
        stopPos.Set(cornerPosition.x + (player.facingDirection*pData.stopOffset.x),cornerPosition.y + pData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();
        isHanging=false;
        if(isClimbing)
        {
            player.transform.position=stopPos;
            isClimbing=false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(isAnimationFinished)
        {
            pSMachine.ChangeState(player.idleState);
        }else
        {
            xInput=player.InputHandler.NormInputX;
            yInput=player.InputHandler.NormInputY;
            jumpInput=player.InputHandler.JumpInput;
        
            player.SetVelocityZero();
            player.transform.position=startPos;

            if(xInput==player.facingDirection && isHanging && !isClimbing)
            {
                isClimbing=true;
                player.anim.SetBool("ClimbLedge",true);
            }else if(yInput==-1 && isHanging && !isClimbing)
            {
                pSMachine.ChangeState(player.airState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.wallJumpState.DetermineWallJumpDirection(true);
                pSMachine.ChangeState(player.wallJumpState);
            }
        }
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPosition = pos;

}
