using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerStats : MonoBehaviour
{
[SerializeField]
private float maxHealth;

public float currentHealth;

private GameManager GM;










private void Start()
{
    currentHealth=maxHealth;
    GM=GameObject.Find("GameManager").GetComponent<GameManager>();

    
    
}

public void DecreaseHealth(float amount)
{
    currentHealth-=amount;

    if(currentHealth<=0.0f)
    {
        StartCoroutine(DeathCoroutine()); 
    }
}
private void Die()
{
    
    Destroy(gameObject);
    GM.Respawn();
}
IEnumerator DeathCoroutine()
{
    yield return new WaitForSeconds(2);
    Die();
}

}
