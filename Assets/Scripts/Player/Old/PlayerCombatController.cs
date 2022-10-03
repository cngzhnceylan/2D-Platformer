using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
  public Transform AttackHitBoxPos;
  public LayerMask WhatIsDamagable;
  public float attackRadius;
  public float attackDamage;
  public static PlayerCombatController instance;
  public bool canReceieveInput;
  public bool inputReceived;
  public bool isAttacking;
  private AttackDetails attackDetails;
  private PlayerController PC;
  private PlayerStats PS;
  


private void Awake()
{
    instance=this;
    
}
private void Start()
{
    PC=GetComponent<PlayerController>();
    PS=GetComponent<PlayerStats>();
}
private void Update()
{   
    Attack();
}
public void Attack()
{
    if(Input.GetMouseButtonDown(0))
    {
        if(canReceieveInput)
        {
            inputReceived=true;
            canReceieveInput=false;
            isAttacking=true;
            CheckAttackHitBox();
        }
        else
        {
            return;
        }
    }
}
private void CheckAttackHitBox()
{
    Collider2D[] detectedObjects=Physics2D.OverlapCircleAll(AttackHitBoxPos.position,attackRadius,WhatIsDamagable);
    attackDetails.damageAmount=attackDamage;
    attackDetails.position=transform.position;
    
    foreach(Collider2D collider in detectedObjects)
    {
        collider.transform.parent.SendMessage("Damage",attackDetails);
        //Instantiate hit particle xd
    }
}

public void InputManager()
{
    if(!canReceieveInput)
    {
        canReceieveInput=true;
    }
    else
    {
        canReceieveInput=false;
    }
}
private void Damage(AttackDetails attackDetails)
{
    if(!PC.GetDashStatus())
    {
        int direction;
        PS.DecreaseHealth(attackDetails.damageAmount);
        if(attackDetails.position.x< transform.position.x)
            {
                direction=1;

            }else
            {
                direction=-1;
            }
        PC.Knockback(direction);
    }
}
    
private void OnDrawGizmos()
{
    Gizmos.DrawWireSphere(AttackHitBoxPos.position,attackRadius);
}

}
