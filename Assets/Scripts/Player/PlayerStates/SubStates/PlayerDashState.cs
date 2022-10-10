using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : AbilityState
{
    public bool canDash{get; private set;}
    private bool isHolding;
    private bool dashInputStop;
    private float lastDashTime;
    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private Vector2 lastAfterImagePos;
    public PlayerDashState(Player player, PSMachine pSMachine, PData pData, string animBoolName) : base(player, pSMachine, pData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canDash=false;
        player.InputHandler.UseDashInput();
        isHolding=true;
        dashDirection=Vector2.right*player.facingDirection;

        Time.timeScale=pData.holdTimeScale;
        startTime=Time.unscaledTime;

        player.dashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        if(player.currentVelocity.y > 0)
        {
            player.SetVelocityY(player.currentVelocity.y * pData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isExitingState)
        {
            player.anim.SetFloat("yVelocity",player.currentVelocity.y);
            player.anim.SetFloat("xVelocity",Mathf.Abs(player.currentVelocity.x));
            if(isHolding)
            {
                dashDirectionInput=player.InputHandler.DashDirectionInput;
                dashInputStop=player.InputHandler.dashInputStop;

                if(dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }
                float angle=Vector2.SignedAngle(Vector2.right,dashDirection);
                player.dashDirectionIndicator.rotation=Quaternion.Euler(0f,0f,angle -45f);

                if(dashInputStop || Time.unscaledTime >= startTime + pData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime=Time.time;
                    player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                    player.RB.drag = pData.drag;
                    player.SetVelocity(pData.dashVelocity,dashDirection);
                    player.dashDirectionIndicator.gameObject.SetActive(false);
                    PlaceAfterImage();
                }
            }
            else
            {
                player.SetVelocity(pData.dashVelocity,dashDirection);
                CheckIfShouldPlaceAfterImage();
                if(Time.time >= startTime + pData.dashTime)
                {
                    player.RB.drag=0f;
                    isAbilityDone=true;
                    lastDashTime=Time.time;
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position,lastAfterImagePos) >= pData.distanceBetweenAfterImg)
        {
            PlaceAfterImage();
        }
    }
    private void PlaceAfterImage()
    {
        AfterImagePool.Instance.GetFromPool();
        lastAfterImagePos=player.transform.position;
    }
    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + pData.dashCooldown;
    }

    public void ResetCanDash() => canDash = true;



}
