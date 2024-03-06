using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{

    private bool isDead = false;
    private float timeToNextAttack;
    private float timeSinceLastAttack = 0;

    protected abstract float maxTimeBetweenAttacks { get; }
    protected abstract float minTimeBetweenAttacks { get; }
    protected abstract int health { get; set; }
    protected abstract int scoreValue { get; }



    protected Animator enemyAnimator;
    public GameObject enemyBulletPrefab;
    private GameObject player;
    protected Rigidbody2D rb;
    private Vector2 movementAddition = Vector2.zero;


    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartAnimation();
        timeToNextAttack = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
    }

    public abstract void StartAnimation();

    public abstract Vector2 getMove();


    private void Update()
    {
        if (!GameManager.instance.isPlaying || isDead)
        {
            return;
        }
        movementAddition += getMove();
        //this isn't perfect, since the time between atacks is also dependent on the frame rate, but it has randomness anyway and the difference will never be noticeable.
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= timeToNextAttack)
        {
            Attack();
            timeSinceLastAttack = 0;
            timeToNextAttack = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        }
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + movementAddition);
        movementAddition = Vector2.zero;

    }

    private void Attack()
    {
        EnemyBullet bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation).GetComponent<EnemyBullet>();
        bullet.SetSpeed((player.transform.position - bullet.transform.position).normalized);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Die();
        }
        //collision with map is specific to only some enemies
        //collisions with bullets are handled in the bullet class
    }

    public void Die()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        enemyAnimator.enabled = true; // some enemies turn off the animator in favor of manual sprite changes
        isDead = true;
        enemyAnimator.SetTrigger("die");
        rb.isKinematic = true; // in case it's dynamic, don't have the animation just fall
        GetComponent<Collider2D>().enabled = false; //so you won't die after the enemy is already dead and the animation is still playing
        yield return new WaitForSeconds(enemyAnimator.GetCurrentAnimatorStateInfo(0).length); //wait for the animation to finish before destroying the object
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.AddScore(scoreValue);
            Die();
        }
    }
}
