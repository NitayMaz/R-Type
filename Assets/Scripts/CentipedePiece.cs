using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class CentipedePiece : Enemy
{
    public float speed;
    private Vector3 velocity;

    public override Vector3 getMove()
    {
        return velocity;
    }

    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("centiPart");
        enemyAnimator.SetFloat("velocityY", velocity.y);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Map"))
        {
            velocity = new Vector3(velocity.x, -velocity.y, 0);
            StartAnimation();
        }
    }

    public void SetSpeed(Vector3 normalizedDirection)
    {
        velocity = normalizedDirection * speed;
        Debug.Log($"set speed to {velocity}");
    }
}
*/