using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circler : Enemy
{
    protected override float maxTimeBetweenAttacks => 25f;

    protected override float minTimeBetweenAttacks => 3f;

    protected override int health { get; set; } = 0; //this one takes damage in a different way

    protected override int scoreValue => 100;

    public Transform circleCenter;
    private Vector2 circleCenterPoint;
    public Sprite aliveSprite;
    public Sprite deadSprite;
    public Sprite coreSprite;
    public bool isCore = false;
    public float distanceFromCenter = 5;
    public float rotationSpeed = 0;
    public Circler rightNeighbor;
    public Circler leftNeighbor;
    public float timeToDestroyNeighbors = 0.5f;


    public override void TakeDamage(int damage)
    {
        // damage is only taken by the core
        if (isDead || !isCore)
            return;
        StartCoroutine(Destroy());

    }

    private IEnumerator Destroy()
    {
        if (isDead)
            yield break;
        isDead = true;
        StartCoroutine(DestroyNeighbours());
        enemyAnimator.enabled = true;
        enemyAnimator.SetTrigger("die");
        //wait for the animation to finish before changing to the dead sprite
        yield return null;//necesary to get into the death animation
        while(enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        enemyAnimator.enabled = false;
        spriteRenderer.sprite = deadSprite;
    }
    private IEnumerator DestroyNeighbours()
    {
        yield return new WaitForSeconds(timeToDestroyNeighbors);
        if (rightNeighbor)
            StartCoroutine(rightNeighbor.Destroy());
        if (leftNeighbor)
            StartCoroutine(leftNeighbor.Destroy());
    }

    private void RotateToCenter()
    {
        Vector2 directionToCenter = circleCenterPoint - rb.position;

        // Calculate angle to face the center point
        float angle = Mathf.Atan2(directionToCenter.normalized.y, directionToCenter.normalized.x) * Mathf.Rad2Deg - 90f; // Subtract 90 degrees to correct for the default sprite facing direction
        float angleInRadians = angle * Mathf.Deg2Rad;
        // Apply rotation to face the center point
        rb.MoveRotation(angle);
    }

    private void MoveAroundCenter()
    {
        // Calculate the angle change based on rotation speed and time
        float angle = rotationSpeed * Time.fixedDeltaTime;

        // Get current position relative to the center point
        Vector2 relativePosition = rb.position - circleCenterPoint;

        // Calculate the new position using a rotation matrix
        // This is equivalent to applying a 2D rotation around the origin (center point)
        float cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector2 newPosition = new Vector2(
            cosAngle * relativePosition.x - sinAngle * relativePosition.y,
            sinAngle * relativePosition.x + cosAngle * relativePosition.y
        );

        // Translate the new position back to the global coordinate space
        newPosition += circleCenterPoint;

        // Apply the new position to the Rigidbody2D
        rb.MovePosition(newPosition);

    }
    public override void Move()
    {
        //any code duplication these 2 function may have is worth it for the sake of simplicity
        MoveAroundCenter();
        RotateToCenter();
    }

    protected override void FixedUpdate()
    {
        //this objects keep moving even after they die
        if (GameManager.instance.isPlaying)
        {
            Move();
        }
    }

    protected override void Attack()
    {
        EnemyBullet bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation).GetComponent<EnemyBullet>();
        Vector2 bulletPosition = new Vector2(bullet.transform.position.x, bullet.transform.position.y);  
        bullet.SetSpeed((circleCenterPoint - bulletPosition).normalized); // shoot into circle not towrds player
    }

    public override void StartAnimation()
    {
        enemyAnimator.enabled = false; // this allows the sprite to be changed manually
        if(isCore)
        {
            spriteRenderer.sprite = coreSprite;
        }
        else
        {
            spriteRenderer.sprite = aliveSprite;
        }
        circleCenterPoint = circleCenter.position;
    }

}
