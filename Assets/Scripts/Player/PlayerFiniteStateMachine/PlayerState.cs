using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Player player;
    protected PSMachine pSMachine;
    protected PData pData;
    protected bool isAnimationFinished;
    protected float startTime;
    private string animBoolName;

    public PlayerState(Player player,PSMachine pSMachine,PData pData, string animBoolName)
    {
        this.player=player;
        this.pSMachine=pSMachine;
        this.pData=pData;
        this.animBoolName=animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime=Time.time;
        player.anim.SetBool(animBoolName,true);
        isAnimationFinished=false;
        Debug.Log(pSMachine.CurrentState);
        
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);
    }

    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks()
    {

    }
    public virtual void AnimationTrigger()
    {

    }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
