using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    public Vector2 RawMovementInput{get; private set;}
    public Vector2 RawDashDirectionInput{get; private set;}
    public Vector2Int DashDirectionInput{get; private set;}
    public int NormInputX{get; private set;}
    public int NormInputY{get; private set;}
    public bool JumpInput{get; private set;}
    public bool JumpInputStop{get; private set;}
    public bool dashInput{get; private set;}
    public bool dashInputStop{get; private set;}
    public bool grabInput{get;private set;}
    [SerializeField] float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    void Start()
    {
        playerInput=GetComponent<PlayerInput>();
        cam=Camera.main;
    }
    void Update(){
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
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
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            grabInput=true;
        }
        if(context.canceled)
        {
            grabInput=false;
        }
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            dashInput=true;
            dashInputStop=false;
            dashInputStartTime=Time.time;
        }
        else if(context.canceled)
        {
            dashInputStop=true;
        }
    }
    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput=context.ReadValue<Vector2>();
        if(playerInput.currentControlScheme=="Keyboard")
        {
            RawDashDirectionInput=cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
            DashDirectionInput=Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => dashInput=false;
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >=jumpInputStartTime + inputHoldTime)
        {
            JumpInput=false;
        }
    }
    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            dashInput=false;
        }
    }
    

}
