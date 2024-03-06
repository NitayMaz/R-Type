using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedePiece : Enemy
{
    public Sprite moveUp;
    public Sprite moveDown;
    public bool moveUpwards = true;
    public float timeLeftToChangeDirection = 0.1f;
    private SpriteRenderer spriteRenderer;
    public float speedX = -3;


    protected override float maxTimeBetweenAttacks => 50f;

    protected override float minTimeBetweenAttacks => 2f;

    protected override int health { get; set; } = 2;

    protected override int scoreValue => 100;


    protected override void OnTriggerEnter2D(Collider2D collision)
    { // die when hitting walls
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
    public override Vector2 getMove()
    {
        // moves sorta like a wave, but a lot more clunky
        timeLeftToChangeDirection -= Time.deltaTime;
        if (timeLeftToChangeDirection <= 0)
        {
            Debug.Log("1:" +spriteRenderer.sprite);
            moveUpwards = !moveUpwards;
            timeLeftToChangeDirection = 1;
            spriteRenderer.sprite = moveUpwards ? moveUp : moveDown;
            Debug.Log("2:" + spriteRenderer.sprite);
        }
        return new Vector2(speedX*Time.deltaTime, moveUpwards? Time.deltaTime : -Time.deltaTime);
    }

    public override void StartAnimation()
    {
        enemyAnimator.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // sprites are changed manually in move
    }
}
