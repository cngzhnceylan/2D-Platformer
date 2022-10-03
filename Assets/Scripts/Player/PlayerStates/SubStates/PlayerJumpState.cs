using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : AbilityState
{
    private int amountOfJumpsLeft;
    public PlayerJumpState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
        amountOfJumpsLeft=pData.amountOfJumps;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityY(pData.jumpVelocity);
        isAbilityDone=true;
        amountOfJumpsLeft--;
        player.airState.SetIsJumping();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    public bool CanJump()
    {
        if(amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = pData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
