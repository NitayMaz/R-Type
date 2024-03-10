using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float minDistanceFromEdgesY = 0.5f;
    public float minDistanceFromEdgesX = 1f;
    public float moveSpeed = 5f;
    public float radiusForHyperSpace = 3f;
    private Animator playerAnimator;
    public Shooter shooter;
    private Rigidbody2D rb;
    private Camera gameCamera;
    private AudioSource audioSource;
    public AudioClip warpSound;
    public AudioClip deathSound;
    public bool isInvincible = false;
    public bool warping = false;
    private bool shouldWarp = false;


    private Vector2 movementAddition;
    private float minCameraX, minCameraY, maxCameraX, maxCameraY;




    // Start is called before the first frame update
    void Awake()
    {
        gameCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isInvincible = !isInvincible;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !warping)
        {
            StartCoroutine(HyperSpace());
        }
        handleMovement();
    }

    void handleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        // the way the axises work is a bit annoying, since they get values that aren't -1 or 1 after you press, so i only account for the actual press values
        if (verticalInput == 1)
        {
            playerAnimator.SetBool("movingUp", true);
            movementAddition += Vector2.up * moveSpeed * Time.deltaTime;
        }
        else if(verticalInput == -1)
        {
            playerAnimator.SetBool("movingDown", true);
            movementAddition += Vector2.down * moveSpeed * Time.deltaTime;
        }
        else
        {
            playerAnimator.SetBool("movingUp", false);
            playerAnimator.SetBool("movingDown", false);
        }
        if (horizontalInput == -1)
        {
            movementAddition += Vector2.left * moveSpeed * Time.deltaTime;
        }
        if (horizontalInput == 1)
        {
            movementAddition += Vector2.right * moveSpeed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if(!GameManager.instance.isPlaying)
        {
            return;
        }
        // make sure the player doesn't go off screen and move him
        CalculateCameraBounds();
        Vector2 newPosition = rb.position + movementAddition;
        newPosition += Vector2.right * GameManager.instance.difficulty * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraX + minDistanceFromEdgesX, maxCameraX - minDistanceFromEdgesX);
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraY + minDistanceFromEdgesY, maxCameraY - minDistanceFromEdgesY);
        rb.MovePosition(newPosition);
        movementAddition = Vector2.zero;    
    }

    public void Die()
    {
        //reset animations
        shooter.ResetShooter();
        playerAnimator.SetBool("movingUp", false);
        playerAnimator.SetBool("movingDown", false);
        StartCoroutine(GameManager.instance.PlayerDied());
    }

    public IEnumerator PlayDeathAnimation()
    {
        audioSource.PlayOneShot(deathSound);
        playerAnimator.SetTrigger("die");
        yield return null; // give time to set into animation
        //wait until the death animation is finished
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
    public IEnumerator PlayOpeningAnimation()
    {   
        //first I need to pre-warm the animation so it shows in full, then play it in full
        playerAnimator.Play("Entering Screen",-1,0);
        playerAnimator.Update(0.01f);
        playerAnimator.StopPlayback();
        playerAnimator.Play("Entering Screen");
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }

    public void RespawnPlayer(Vector2 position)
    {
        playerAnimator.Play("Idle");
        transform.position = position;
    }

    private Vector2 FindSafeLocation()
    {
        bool locationFound = false;
        Vector2 safeLocation = Vector2.zero;
        int attempts = 0;
        CalculateCameraBounds();

        while (!locationFound && attempts < 500) // Limit attempts to avoid an infinite loop
        {
            float randomX = Random.Range(minCameraX + minDistanceFromEdgesX, maxCameraX - 3*minDistanceFromEdgesX); // warping close to the edge is a bit more dangerous
            float randomY = Random.Range(minCameraY + minDistanceFromEdgesY, maxCameraY - minDistanceFromEdgesY);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            Collider2D hit = Physics2D.OverlapCircle(randomPosition, radiusForHyperSpace);
            if (hit == null) // No collision found, position is safe
            {
                safeLocation = randomPosition;
                locationFound = true;
                Debug.Log("Safe location found after " + attempts + " attempts");
            }

            attempts++;
        }
        return safeLocation;
    }

    private IEnumerator HyperSpace()
    {
        warping = true;
        audioSource.PlayOneShot(warpSound);
        Vector2 safeLocation = FindSafeLocation();
        playerAnimator.SetTrigger("hyperSpace");
        yield return new WaitWhile(() => shouldWarp == false); // changed by animation event
        shouldWarp = false;
        if (safeLocation != Vector2.zero)
        {
            rb.position = safeLocation;
        }
        else
        {
            Debug.Log("HyperSpace failed!");
        }
        yield return new WaitWhile(() => playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        warping = false;

    }
    private void SetWarp()
    {
        shouldWarp = true;
    }
}
