using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    public float attack_frequency;
    public float timeSinceLastAttack;
    public int health;
    public Animator enemyAnimator;


    private void Start()
    {
        StartAnimation();
    }

    public abstract void StartAnimation();


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Die();
            Die();
        }
        if (collision.gameObject.tag == "Map")
        {
            Die();
        }
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("die");
        //maybe need to wait for the animation to finish before destroying the object
        Destroy(gameObject);
    }

   
}
