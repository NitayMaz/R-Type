using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha : Enemy
{
    protected override float maxTimeBetweenAttacks => 5.0f;

    protected override float minTimeBetweenAttacks => 1.0f;

    protected override int health { get; set; } = 3;

    protected override int scoreValue => 500;

    public bool moveLeft;

    public override void Move()
    {
        Vector2 movement = new Vector2(moveLeft ? -2 : 2, 0) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("mecha");
        enemyAnimator.SetBool("walk left", moveLeft);
    }

    public void SwitchDirection()
    {
        moveLeft = !moveLeft;
        enemyAnimator.SetBool("walk left", moveLeft);
    }
}
      
