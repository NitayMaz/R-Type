using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator playerAnimator;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.SetBool("movingUp", true);
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("movingUp", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", true);
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetBool("movingDown", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    
}
