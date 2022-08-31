using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Transform wallCheck;
    public Vector2 wallHopDirection = new Vector2(1,0.5f);
    public Vector2 wallJumpDirection=new Vector2(1,2);
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    private int amountOfJumps=2;
    public int amountOfJumpsLeft;
    private int facingDirection=1;
    private int lastWallJumpDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float movementInputDirection;
    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash=-100f;
    public float movementSpeed=9.0f;
    public float jumpForce=18.0f;
    public float groundCheckRadius=0.3f;
    public float wallCheckDistance=0.6f;
    public float wallSlideSpeed=1.0f;
    public float movementForceInAir=18.0f;
    public float jumpTimerSet=0.15f;
    public float turnTimerSet=0.1f;
    public float wallJumpTimerSet=0.5f;
    public float airDragMultiplier=0.95f;
    private float variableJumpHeightMultiplier=0.5f;
    public float wallHopForce=2;
    public float wallJumpForce=20.2f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset1=0f;
    public float ledgeClimbYOffset2=0f;
    public float dashTime=0.2f;
    public float dashSpeed=30.0f;
    public float distanceBetweenImages=0.1f;
    public float dashCoolDown=2.5f;
    private bool isFacingRight=true;
    private bool isRunning;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isWallSliding;
    public bool canNormalJump;
    public bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool isDashing=false;
    public bool hasWallJumped;
    public bool isDoubleJumped;
    public bool canDoubleJump;
    public bool isTouchingLedge;
    public bool canClimbLedge=false;
    public bool ledgeDetected;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        amountOfJumpsLeft=amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void CheckIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y<0 && !canClimbLedge)
        {
            isWallSliding=true;
            amountOfJumpsLeft=1;
           
        }
        else{
            isWallSliding=false;
        }
    }
    public void FinishLedgeClimb()
    {
        canClimbLedge=false;
        transform.position=ledgePos2;
        canMove=true;
        canFlip=true;
        ledgeDetected=false;
        anim.SetBool("canClimbLedge",canClimbLedge);
    }
    private void CheckSurroundings(){
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,whatIsGround);
        isTouchingWall=Physics2D.Raycast(wallCheck.position,transform.right,wallCheckDistance,whatIsGround);
        isTouchingLedge=Physics2D.Raycast(ledgeCheck.position,transform.right,wallCheckDistance,whatIsGround);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected=true;
            ledgePosBot=wallCheck.position;
        }
   
    }
    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge=true;

            if(isFacingRight)
            {
                ledgePos1=new Vector2(Mathf.Floor(ledgePosBot.x+wallCheckDistance)-ledgeClimbXOffset1,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset1);
                ledgePos2=new Vector2(Mathf.Floor(ledgePosBot.x+wallCheckDistance)+ ledgeClimbXOffset2,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1=new Vector2(Mathf.Ceil(ledgePosBot.x-wallCheckDistance)+ledgeClimbXOffset1+0.4f,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset1);
                ledgePos2=new Vector2(Mathf.Ceil(ledgePosBot.x-wallCheckDistance)-ledgeClimbXOffset2+0.4f,Mathf.Floor(ledgePosBot.y)+ledgeClimbYOffset2);
            }
            canMove=false;
            canFlip=false;
        }
        if(canClimbLedge)
        {
            transform.position=ledgePos1;
        }
    }
    private void CheckIfCanJump()
    {
        if(isGrounded&& rb.velocity.y<=0){
            amountOfJumpsLeft=amountOfJumps;
        }
        if(isTouchingWall)
        {
            canWallJump=true;
        }
        if(amountOfJumpsLeft<=0)
        {
            canNormalJump=false;
            isDoubleJumped=false;
        }
        else
        {
            canNormalJump=true;
            canDoubleJump=true;
        }
    }
    private void CheckMovementDirection()
    {
        if(isFacingRight&&movementInputDirection<0){
            Flip();
        }
        else if(!isFacingRight&&movementInputDirection>0){
            Flip();     
        }
        if(rb.velocity.x==0){
            isRunning=false;
            
        }
        else{
            isRunning=true;
        }
    }
    private void UpdateAnimations()
    {
        anim.SetBool("isRunning",isRunning);
        anim.SetBool("isGrounded",isGrounded);
        anim.SetFloat("velocityY",rb.velocity.y);
        anim.SetFloat("velocityX",rb.velocity.x);
        anim.SetBool("isWallSliding",isWallSliding);
        anim.SetBool("isDoubleJumped",isDoubleJumped);
        anim.SetBool("canClimbLedge",canClimbLedge);
        anim.SetBool("isDashing",isDashing);
    }
    private void CheckInput()
    {
        movementInputDirection=Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }else if(amountOfJumpsLeft==1)
            {
                DoubleJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        if(Input.GetButtonDown("Horizontal") && isTouchingWall )
        {
            if(!isGrounded && movementInputDirection!= facingDirection)
            {
                canMove=false;
                canFlip=false;

                turnTimer=turnTimerSet;
            }
        }
        if(turnTimer >= 0)
        {
            turnTimer-=Time.deltaTime;
            if(turnTimer<=0)
            {
                canMove=true;
                canFlip=true;
            }
        }

        if(checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier=false;
            rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y*variableJumpHeightMultiplier);
        }
        if(Input.GetButtonDown("Dash"))
        {
            if(Time.time >=(lastDash+dashCoolDown))
            {
                AttempToDash();
            }
            
        }
    }
    private void AttempToDash()
    {
        isDashing=true;
        dashTimeLeft=dashTime;
        lastDash=Time.time;

        AfterImagePool.Instance.GetFromPool();
        lastImageXPos=transform.position.x;
    }
    private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                    canMove=false;
                    canFlip=false;
                    rb.velocity=new Vector2(dashSpeed*facingDirection,rb.velocity.y);
                    dashTimeLeft-=Time.deltaTime;

                    if(Mathf.Abs(transform.position.x-lastImageXPos)>distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    lastImageXPos=transform.position.x;
                }
            }
            if(dashTimeLeft<=0 || isTouchingWall)
            {
                isDashing=false;
                canMove=true;
                canFlip=true;
            }
        }
    }

    private void ApplyMovement()
    {
        
        if(!isGrounded && !isWallSliding && movementInputDirection==0)
        {
            rb.velocity=new Vector2(rb.velocity.x*airDragMultiplier, rb.velocity.y);
        }
        else if(canMove)
        {
             rb.velocity=new Vector2(movementSpeed*movementInputDirection,rb.velocity.y);
        }
        if(isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity=new Vector2(rb.velocity.x,-wallSlideSpeed);
            }
        }
    }
    public void DisableFlip()
    {
        canFlip=false;
    }
    public void EnableFlip()
    {
        canFlip=true;
    }
    private void Flip(){
        if(!isWallSliding && canFlip)
        {
            facingDirection*=-1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f,180.0f,0.0f);
        }
        
    }
    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            //WallJump
            if(!isGrounded && isTouchingWall && movementInputDirection!=0 && movementInputDirection != facingDirection)
            {
                WallJump();

            }
            else if(isGrounded)
            {
                NormalJump();
            }
            else if(amountOfJumpsLeft==1)
            {
                DoubleJump();
            }
        }
        if(isAttemptingToJump)
        {
            jumpTimer-=Time.deltaTime;
        }
        if(wallJumpTimer > 0)
        {
            if(hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity=new Vector2(rb.velocity.x,-8);
                Debug.Log("Düşmeli");
                hasWallJumped=false;
               
            }
            else if(wallJumpTimer<=0)
            {
                hasWallJumped=false;
                
            }else
            {
                wallJumpTimer-=Time.deltaTime;
            }
        }
    }
    private void NormalJump()
    {
        if(canNormalJump && !isWallSliding)
        {
            rb.velocity=new Vector2(rb.velocity.x,jumpForce);
            amountOfJumpsLeft--;
            jumpTimer=0;
            isAttemptingToJump=false;
            checkJumpMultiplier=true;
        }
    }
    private void WallJump()
    {
        if(canWallJump)
        {
            rb.velocity= new Vector2(rb.velocity.x,0.0f);
            isWallSliding=false;
            amountOfJumpsLeft=1;
            amountOfJumpsLeft--;
            Vector2 forceToAdd= new Vector2(wallJumpForce*wallJumpDirection.x*movementInputDirection,wallJumpForce*wallJumpDirection.y);
            rb.AddForce(forceToAdd,ForceMode2D.Impulse);
            jumpTimer=0;
            isAttemptingToJump=false;
            checkJumpMultiplier=true;
            turnTimer=0;
            canMove=true;
            canFlip=true;
            hasWallJumped=true;
            wallJumpTimer=wallJumpTimerSet;
            lastWallJumpDirection=-facingDirection;
        }
    }
    private void DoubleJump()
    {
        if(canDoubleJump){
        rb.velocity=new Vector2(rb.velocity.x,jumpForce);
        amountOfJumpsLeft--;
        isWallSliding=false;
        jumpTimer=0;
        isAttemptingToJump=false;
        checkJumpMultiplier=true;
        isDoubleJumped=true;
        }
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x+wallCheckDistance,wallCheck.position.y,wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x+wallCheckDistance,ledgeCheck.position.y,ledgeCheck.position.z));
    }

}
