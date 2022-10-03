using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : GroundedState
{
    
    public PlayerLandState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(inputX !=0 && isGrounded)
        {
            pSMachine.ChangeState(player.moveState);
        }
        else if(isAnimationFinished)
        {
            pSMachine.ChangeState(player.idleState);
        }

    }

}
