using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
[SerializeField]
private float activeTime=0.1f;
private float timeActivated;
private float alpha;
[SerializeField]
private float alphaSet=0.8f;
private float alphaMultiplier=0.50f;
[SerializeField]
private Transform player;
public SpriteRenderer sR;
public SpriteRenderer playerSr;
public Color color;



private void OnEnable()
{
    sR=GetComponent<SpriteRenderer>();
    player=GameObject.FindGameObjectWithTag("Player").transform;
    playerSr=GameObject.Find("Player").GetComponent<SpriteRenderer>();

    alpha=alphaSet;
    sR.sprite=playerSr.sprite;
    transform.position=player.position;
    transform.rotation=player.rotation;
    timeActivated=Time.time;
}
private void Update()
{
    alpha *=alphaMultiplier;
    color=new Color(1.0f,1.0f,1.0f,alpha);
    sR.color=color;

    if(Time.time >= (timeActivated + activeTime))
    {
        AfterImagePool.Instance.AddToPool(gameObject);
    }
}

}
