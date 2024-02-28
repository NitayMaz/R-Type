using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator playerAnimator;
    public bool alive = true;
    public Rigidbody2D rb;

    private Vector3 movementAddition = Vector3.zero;




    // Start is called before the first frame update
    void Start()
    {
        alive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemy and enemy bullets collisions are handled in those classes
        if (collision.gameObject.tag == "Map")
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isPlaying)
        {
            return;
        }
        handleMovement();
    }

    void handleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.SetBool("movingUp", true);
            movementAddition += Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("movingUp", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", true);
            movementAddition += Vector3.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementAddition += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementAddition += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movementAddition);
        movementAddition = Vector3.zero;
    }

    public void Die()
    {
        StartCoroutine(PlayDeathAnimation());
    }

    IEnumerator PlayDeathAnimation()
    {
        GameManager.instance.isPlaying = false;
        playerAnimator.SetTrigger("die");
        //wait until the death animation is finished
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    
}
