using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Animator bulletAnimator;
    public float speed = 3f;
    private Vector3 movementDelta;


    // Start is called before the first frame update
    void Start()
    {
        bulletAnimator.SetBool("enemyBullet", true);
    }

    public void SetSpeed(Vector3 directionToTarget)
    {
        //directionToTarget is normalized
        movementDelta = directionToTarget*speed*Time.deltaTime;
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

    private void Update()
    {
        if (!GameManager.instance.isPlaying)
        {
            return;
        }
        transform.position += movementDelta;
    }
}
