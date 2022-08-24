using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float moveSpeed=5.0f;
    private float movementInputDirection;
    public bool isRunning;
    public bool isFacingRight=true;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        ApplyMovement();
        UpdateAnimations();
    }
    private void CheckInput()
    {
        movementInputDirection=Input.GetAxisRaw("Horizontal");
    }
    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection<0)
        {
            Flip();
        }
        if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if(rb.velocity.x==0)
        {
            isRunning=false;

        }else
        {
            isRunning=true;
        }
    }
    private void ApplyMovement()
    {
        rb.velocity=new Vector2(moveSpeed*movementInputDirection,rb.velocity.y);
    } 
    private void UpdateAnimations()
    {
        anim.SetBool("isRunning",isRunning);
    }
    private void Flip()
    {
        isFacingRight=!isFacingRight;
        transform.Rotate(0,180.0f,0);
    }
}
