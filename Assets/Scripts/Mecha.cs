using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha : Enemy
{
    protected override float maxTimeBetweenAttacks => 7.0f;

    protected override float minTimeBetweenAttacks => 1.0f;

    protected override int health { get; set; } = 3;

    protected override int scoreValue => 500;

    public bool moveLeft;

    public override Vector2 getMove()
    {
        return new Vector2(moveLeft ? -2 : 2, 0) * Time.deltaTime;
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
      
