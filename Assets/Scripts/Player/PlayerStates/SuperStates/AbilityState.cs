using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : PlayerState
{
    protected bool isAbilityDone;
    private bool isGrounded;
    public AbilityState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
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
        isAbilityDone=false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(isAbilityDone)
        {
            if(isGrounded && player.currentVelocity.y < 0.01f)
            {
                pSMachine.ChangeState(player.idleState);
            }
            else
            {
                pSMachine.ChangeState(player.airState);
            }
        }
        
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
