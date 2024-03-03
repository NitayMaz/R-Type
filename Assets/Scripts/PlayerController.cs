using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float minDistanceFromEdgesY = 0.5f;
    public float minDistanceFromEdgesX = 1f;
    public float moveSpeed = 5f;
    public Animator playerAnimator;
    public bool alive = true;
    private Rigidbody2D rb;
    public Camera gameCamera;

    private Vector2 movementAddition;
    private float minCameraX, minCameraY, maxCameraX, maxCameraY;




    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() 
    { 
        alive = true;
        CalculateCameraBounds();
    }
    
    private void CalculateCameraBounds()
    {
        Vector2 bottomLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minCameraX = bottomLeft.x;
        minCameraY = bottomLeft.y;
        maxCameraX = topRight.x;
        maxCameraY = topRight.y;

    
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
            movementAddition += Vector2.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("movingUp", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", true);
            movementAddition += Vector2.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementAddition += Vector2.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementAddition += Vector2.right * moveSpeed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // make sure the player doesn't go off screen and move him
        Vector2 newPosition = rb.position + movementAddition;
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraX + minDistanceFromEdgesX, maxCameraX - minDistanceFromEdgesX);
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraY + minDistanceFromEdgesY, maxCameraY - minDistanceFromEdgesY);
        rb.MovePosition(newPosition);
        movementAddition = Vector2.zero;    }

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
    public IEnumerator PlayOpeningAnimation()
    {   
        //first I need to pre-warm the animation so it shows in full, then play it in full
        playerAnimator.Play("Entering Screen",-1,0);
        playerAnimator.Update(0.01f);
        playerAnimator.StopPlayback();
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Debug.Log("Opening animation finished");
    }
}
