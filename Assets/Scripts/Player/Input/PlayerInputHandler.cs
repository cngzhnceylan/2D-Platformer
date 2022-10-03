using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput{get; private set;}
    public int NormInputX{get; private set;}
    public int NormInputY{get; private set;}
    public bool JumpInput{get; private set;}
    public bool JumpInputStop{get; private set;}
    [SerializeField] float jumpInputHold =0.2f;
    private float jumpInputStartTime;


    void Update(){
        CheckHumpInputHoldTime();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput= context.ReadValue<Vector2>();
        NormInputX=(int)(RawMovementInput*Vector2.right).normalized.x;
        NormInputY=(int)(RawMovementInput*Vector2.up).normalized.y;
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput=true;
            JumpInputStop=false;
            jumpInputStartTime=Time.time;
        }
        if(context.canceled)
        {
            JumpInputStop=true;
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void CheckHumpInputHoldTime()
    {
        if(Time.time >=jumpInputStartTime + jumpInputHold)
        {
            JumpInput=false;
        }
    }

}
