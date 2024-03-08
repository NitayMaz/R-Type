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
    public override void Move()
    {
        // moves sorta like a wave, but a lot more clunky
        timeLeftToChangeDirection -= Time.fixedDeltaTime;
        if (timeLeftToChangeDirection <= 0)
        {
            moveUpwards = !moveUpwards;
            timeLeftToChangeDirection = 1;
            spriteRenderer.sprite = moveUpwards ? moveUp : moveDown;
        }
        rb.MovePosition(rb.position + new Vector2(speedX*Time.fixedDeltaTime, moveUpwards? Time.fixedDeltaTime : -Time.fixedDeltaTime));
    }

    public override void StartAnimation()
    {
        enemyAnimator.enabled = false;
        // sprites are changed manually in move
    }
}
