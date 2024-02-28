using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Enemy
{
    public float movementAmplitude = 2;
    public float movementFrequency = 2;
    public float speedX = 3;

    public override void StartAnimation()
    {
        enemyAnimator.SetTrigger("spinner");
    }

    public override Vector3 Move()
    {
        //moves like a sine wave
        return new Vector3(speedX * Time.deltaTime, movementAmplitude * Mathf.Sin(transform.position.x*movementFrequency), 0);
    }
}
