using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothWalkerController : MonoBehaviour
{
private enum State
{
    Walking,
    Knockback,
    Idle,
    Dead   
}
private State currentState;
private int facingDirection,damageDirection;
private bool groundDetected, wallDetected;
[SerializeField]
private Transform groundCheck,wallCheck,touchDamageCheck;
[SerializeField]
private LayerMask whatIsGround,
whatIsPlayer;
[SerializeField]
private float 
groundCheckDistance,
wallCheckDistance,
movementSpeed,
maxHealth,
knockbackDuration,
lastTouchDamageTime,
touchDamageCooldown,
touchDamage,
touchDamageWidth,
touchDamageHeight;
[SerializeField]
private Vector2 knockbackSpeed;
private GameObject alive;
private Rigidbody2D aliveRb;
private Animator aliveAnim;
private Vector2 movement,touchDamageBotLeft,touchDamageTopRight;
public float currentHealth,knockbackStartTime;
private float[] attackDetails=new float[2];





private void Start()
{
    alive=transform.Find("Alive").gameObject;
    aliveRb=alive.GetComponent<Rigidbody2D>();
    aliveAnim=alive.GetComponent<Animator>();
    currentHealth=maxHealth;
    facingDirection=1;
}
private void Update()
{
    switch(currentState)
    {
        case State.Idle:
            UpdateIdleState();
            break;
        case State.Walking:
            UpdateWalkingState();
            break;
        case State.Knockback:
            UpdateKnockbackState();
            break;
        case State.Dead:
            UpdateDeadState();
            break;
    }
}




//--IdleState
private void EnterIdleState()
{

}
private void UpdateIdleState()
{
    
}
private void ExitIdleState()
{
    
}

//--WalkingState

private void EnterWalkingState()
{
    aliveAnim.SetBool("isWalking",true);
}
private void UpdateWalkingState()
{
    groundDetected=Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
    wallDetected=Physics2D.Raycast(wallCheck.position,transform.right,wallCheckDistance,whatIsGround);
    CheckTouchDamage();

    if(!groundDetected || wallDetected)
    {
        Flip();
    }else
    {
        movement.Set(movementSpeed*facingDirection,aliveRb.velocity.y);
        aliveRb.velocity=movement;
    }
}
private void ExitWalkingState()
{
    aliveAnim.SetBool("isWalking",false);
}
//----KnockbackState
private void EnterKnockbackState()
{
    knockbackStartTime=Time.time;
    movement.Set(knockbackSpeed.x*damageDirection,knockbackSpeed.y);
    aliveRb.velocity=movement;
    aliveAnim.SetBool("Knockback",true);
}
private void UpdateKnockbackState()
{
    if(Time.time>=knockbackStartTime+knockbackDuration)
    {
        SwitchState(State.Walking);
    }
}
private void ExitKnockbackState()
{
    aliveAnim.SetBool("Knockback",false);
}
//--Dead State
private void EnterDeadState()
{
    aliveAnim.SetBool("isDead",true);
    StartCoroutine(Die());
}
private void UpdateDeadState()
{
    aliveAnim.SetBool("isDead",true);
}
private void ExitDeadState()
{
   
}

//--OtherFuntions
private void Flip()
{
    facingDirection*=-1;
    alive.transform.Rotate(0,180.0f,0);

}
private void Damage(float[] attackDetails)
{
    currentHealth-=attackDetails[0];
    if(attackDetails[1]> alive.transform.position.x)
    {
        damageDirection=-1;
    }else
    {
        damageDirection=1;
    }
    //hit particle

    if(currentHealth > 0.0f)
    {
        SwitchState(State.Knockback);
    }else
    {
        SwitchState(State.Dead);
    }
}
private void CheckTouchDamage()
{
    if(Time.time>= lastTouchDamageTime + touchDamageCooldown)
    {
        touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth/2),touchDamageCheck.position.y - (touchDamageHeight/2));
        touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth/2),touchDamageCheck.position.y + (touchDamageHeight/2));

        Collider2D hit =Physics2D.OverlapArea(touchDamageBotLeft,touchDamageTopRight,whatIsPlayer);

        if(hit !=null)
        {
            lastTouchDamageTime =Time.time;
            attackDetails[0]=touchDamage;
            attackDetails[1]=alive.transform.position.x;
            hit.SendMessage("Damage",attackDetails);
        }
    }
}
private void SwitchState(State state)
{
    switch(currentState)
    {
        case State.Idle:
            ExitIdleState();
            break;
        case State.Walking:
            ExitWalkingState();
            break;
        case State.Knockback:
            ExitKnockbackState();
            break;
        case State.Dead:
            ExitDeadState();
            break;
    }

        switch(state)
    {
        case State.Idle:
            EnterIdleState();
            break;
        case State.Walking:
            EnterWalkingState();
            break;
        case State.Knockback:
            EnterKnockbackState();
            break;
        case State.Dead:
            EnterDeadState();
            break;
    }
    currentState=state;
}
private void OnDrawGizmos()
{
    Gizmos.DrawLine(groundCheck.position,new Vector2(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
    Gizmos.DrawLine(wallCheck.position,new Vector2(wallCheck.position.x+wallCheckDistance,wallCheck.position.y));

    Vector2 botLeft= new Vector2(touchDamageCheck.position.x - (touchDamageWidth/2),touchDamageCheck.position.y - (touchDamageHeight/2));
    Vector2 botRight= new Vector2(touchDamageCheck.position.x + (touchDamageWidth/2),touchDamageCheck.position.y - (touchDamageHeight/2));
    Vector2 topRight= new Vector2(touchDamageCheck.position.x + (touchDamageWidth/2),touchDamageCheck.position.y + (touchDamageHeight/2));
    Vector2 topLeft= new Vector2(touchDamageCheck.position.x - (touchDamageWidth/2),touchDamageCheck.position.y + (touchDamageHeight/2));

    Gizmos.DrawLine(botLeft,botRight);
    Gizmos.DrawLine(botRight,topRight);
    Gizmos.DrawLine(topRight,topLeft);
    Gizmos.DrawLine(topLeft,botLeft);
}

private IEnumerator Die()
{
    yield return new WaitForSeconds(aliveAnim.GetCurrentAnimatorStateInfo(0).length);
    Destroy(gameObject);
}
}
