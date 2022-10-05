using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : TouchingWallState
{
    public PlayerWallClimbState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isExitingState)
        {
            player.SetVelocityY(pData.wallClimbVelocity);
            if(yInput!=1)
            {
                pSMachine.ChangeState(player.wallGrabState);
            }
        }
        
    }


}
