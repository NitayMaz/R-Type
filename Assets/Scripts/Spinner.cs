using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Enemy
{
    public float movementAmplitude = 2;
    public float movementFrequency = 2;
    public float speedX = 3;

    protected override float maxTimeBetweenAttacks => 20.0f;

    protected override float minTimeBetweenAttacks => 1.0f;

    protected override int scoreValue => 100;

    protected override int health { get; set; } = 1;


    protected override void OnTriggerEnter2D(Collider2D collision)
    { //die when hitting walls
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }
    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("spinner");
    }

    public override Vector2 getMove()
    {
        //moves like a sine wave
        return new Vector2(speedX * Time.deltaTime, movementAmplitude * Mathf.Sin(rb.position.x*movementFrequency) * Time.deltaTime);
    }
}
