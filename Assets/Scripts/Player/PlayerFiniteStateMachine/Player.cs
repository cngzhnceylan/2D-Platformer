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
    public PlayerWallJumpState wallJumpState {get;private set;}
    public PlayerLedgeClimbState ledgeClimbState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerCrouchIdleState crouchIdleState{get; private set;}
    public PlayerCrouchMoveState crouchMoveState{get; private set;}
    [SerializeField] private PData pData;
    #endregion
    #region Components
    public Animator anim{get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}
    public Rigidbody2D RB{get; private set;}
    public Transform dashDirectionIndicator{get; private set;}
    public BoxCollider2D movementCollider{get; private set;}
    #endregion
    #region Other Variables
    public Vector2 currentVelocity{get; private set;}
    private Vector2 workspace;
    public int facingDirection;
    #endregion
    #region Check Transfroms
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform ledgeCheck;
    [SerializeField] Transform ceilingCheck;
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
        wallJumpState=new PlayerWallJumpState(this,pSMachine,pData,"InAir");
        ledgeClimbState=new PlayerLedgeClimbState(this,pSMachine,pData,"LedgeClimbState");
        dashState=new PlayerDashState(this,pSMachine,pData,"Land");
        crouchIdleState=new PlayerCrouchIdleState(this,pSMachine,pData,"CrouchIdle");
        crouchMoveState=new PlayerCrouchMoveState(this,pSMachine,pData,"CrouchMove");

    }

    private void Start()
    {
        anim=GetComponent<Animator>();
        InputHandler=GetComponent<PlayerInputHandler>();
        RB=GetComponent<Rigidbody2D>();
        movementCollider=GetComponent<BoxCollider2D>();
        dashDirectionIndicator=transform.Find("DashDirectionIndicator");
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
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x*velocity*direction,angle.y*velocity);
        RB.velocity=workspace;
        currentVelocity=workspace;
    }
    public void SetVelocityZero()
    {
        RB.velocity=Vector2.zero;
        currentVelocity=Vector2.zero;
    }
    public void SetVelocity(float velocity,Vector2 direction)
    {
        workspace=direction*velocity;
        RB.velocity=workspace;
        currentVelocity=workspace;
    }
    public void SetColliderHeight(float height)
    {
        Vector2 center=movementCollider.offset;
        workspace.Set(movementCollider.size.x,height);
        center.y+= (height-movementCollider.size.y)/2;
        movementCollider.size=workspace;
        movementCollider.offset=center;
    }
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit=Physics2D.Raycast(wallCheck.position,Vector2.right*facingDirection,pData.wallCheckDistance,pData.whatIsGround);
        float xDist=xHit.distance;
        workspace.Set((xDist+ 0.015f)*facingDirection,0f);
        RaycastHit2D yHit=Physics2D.Raycast(ledgeCheck.position+(Vector3)(workspace),Vector2.down,ledgeCheck.position.y-wallCheck.position.y + 0.015f,pData.whatIsGround);
        float yDist=yHit.distance;
        workspace.Set(wallCheck.position.x + (xDist *facingDirection),ledgeCheck.position.y - yDist);
        return workspace;
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
    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position,pData.groundCheckRadius,pData.whatIsGround);
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,pData.groundCheckRadius,pData.whatIsGround);
    }
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position,Vector2.right*facingDirection,pData.wallCheckDistance,pData.whatIsGround);
    }
    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position,Vector2.right*facingDirection,pData.wallCheckDistance,pData.whatIsGround);
    }
        public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position,Vector2.right*-facingDirection,pData.wallCheckDistance,pData.whatIsGround);
    }

}
