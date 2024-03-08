using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Animator bulletAnimator;
    private Rigidbody2D rb;

    public float speed = 0.5f;
    private Vector2 movementDelta;


    // Start is called before the first frame update
    void Start()
    {
        bulletAnimator.SetBool("enemyBullet", true);
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 directionToTarget)
    {
        //directionToTarget is normalized
        movementDelta = directionToTarget*speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Die();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
        {
            return;
        }
        rb.MovePosition(rb.position + movementDelta*Time.fixedDeltaTime);
    }
}
