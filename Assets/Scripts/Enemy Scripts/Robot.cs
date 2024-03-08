using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy
{
    //no attacks
    protected override float maxTimeBetweenAttacks => 100f;

    protected override float minTimeBetweenAttacks => 100f;

    protected override int health { get; set; } = 2;

    protected override int scoreValue => 250;

    private float yExtraSpeedModifier = 1f;

    public float speed = 4f;



    public override void Move()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        if (direction.x > 0) // can't move right(backwards)
            direction.x = 0;
        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        // it tends to get stuck in a wall. so we give it a little push
        if (direction.y > 0)
        {
            newPosition.y += Vector2.up.y * Time.fixedDeltaTime*yExtraSpeedModifier;
        }
        else
        {
            newPosition.y -= Vector2.up.y * Time.fixedDeltaTime * yExtraSpeedModifier;
        }
        rb.MovePosition(newPosition);
    }

    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("robot");
    }
}
