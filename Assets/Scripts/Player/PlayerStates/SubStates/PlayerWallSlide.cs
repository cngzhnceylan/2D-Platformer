using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : TouchingWallState
{
    public PlayerWallSlide(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isExitingState)
        {
            player.SetVelocityY(-pData.wallSlideVelocity);
            if(grabInput && yInput==0)
            {
                pSMachine.ChangeState(player.wallGrabState);
            }
        }
        
    }

}
