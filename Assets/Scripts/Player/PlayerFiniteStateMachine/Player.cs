using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables


    public PSMachine pSMachine{get; private set;}

    public PlayerIdleState idleState{get; private set;}
    public PlayerMoveState moveState{get; private set;}
    public PlayerJumpState jumpState{get; private set;}
    public PlayerInAirState airState {get; private set;}
    public PlayerLandState landState {get;private set;}
    public PlayerWallSlide wallSlideState {get;private set;}
    public PlayerWallClimbState wallClimbState{get; private set;}
    public PlayerWallGrabState  wallGrabState {get; private set;}
    [SerializeField] private PData pData;
    #endregion
    #region Components
    public Animator anim{get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}
    public Rigidbody2D RB{get; private set;}
    #endregion
    #region Other Variables
    public Vector2 currentVelocity{get; private set;}
    private Vector2 workspace;
    public int facingDirection;
    #endregion
    #region Check Transfroms
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    #endregion

    


    private void Awake()
    {
        pSMachine= new PSMachine();
        idleState= new PlayerIdleState(this,pSMachine,pData,"Idle");
        moveState= new PlayerMoveState(this,pSMachine,pData,"Move");
        jumpState=new PlayerJumpState(this,pSMachine,pData,"InAir");
        airState=new PlayerInAirState(this,pSMachine,pData,"InAir");
        landState=new PlayerLandState(this,pSMachine,pData,"Land");
        wallSlideState=new PlayerWallSlide(this,pSMachine,pData,"WallSlide");
        wallClimbState=new PlayerWallClimbState(this,pSMachine,pData,"WallClimb");
        wallGrabState=new PlayerWallGrabState(this,pSMachine,pData,"WallGrab");

    }

    private void Start()
    {
        anim=GetComponent<Animator>();
        InputHandler=GetComponent<PlayerInputHandler>();
        RB=GetComponent<Rigidbody2D>();
        facingDirection=1;
        pSMachine.Initialize(idleState);
    }

    private void Update()
    {
        currentVelocity=RB.velocity;
        pSMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        pSMachine.CurrentState.PhysicUpdate();
    }
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity,currentVelocity.y);
        RB.velocity= workspace;
        currentVelocity=workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x,velocity);
        RB.velocity=workspace;
        currentVelocity=workspace;
    }
    private void AnimationTrigger() => pSMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => pSMachine.CurrentState.AnimationFinishTrigger();
    private void Flip()
    {
        facingDirection*=-1;
        transform.Rotate(0,180.0f,0);
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,pData.groundCheckRadius,pData.whatIsGround);
    }
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position,Vector2.right*facingDirection,pData.wallCheckDistance,pData.whatIsGround);
    }

}
