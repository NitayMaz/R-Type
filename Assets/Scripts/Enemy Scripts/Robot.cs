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

    public float speed = 4f;



    public override void Move()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("robot");
    }
}
