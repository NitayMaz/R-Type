using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public bool playStartAnimation = true;
    public static GameManager instance;
    private GameObject map;
    private GameObject player;
    private PlayerController playerController;
    private Camera gameCamera;
    private AudioSource audioSource;
    public AudioClip StartGameSound;
    public int score = 0;
    public float cameraMoveSpeed = 2f;

    //isPlaying is used to stop the game when the player dies, I use it instead of time.timeScale because the latter wouldn't stop objects(mainly bullets) from being created
    public bool isPlaying = false;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Singleton violation");
        }
        instance = this;
    }

    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        gameCamera = Camera.main;
        //sort enemies by x position so that we always know the next enemy to spawn
        StartCoroutine(StartNewGame());
    }
    IEnumerator StartNewGame()
    {
        audioSource.PlayOneShot(StartGameSound);
        //wait for animation to finish before starting the game
        if (playStartAnimation)
            yield return StartCoroutine(playerController.PlayOpeningAnimation());
        isPlaying = true;
    }
    

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        gameCamera.transform.position += Vector3.right * cameraMoveSpeed * Time.fixedDeltaTime;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIHandler.instance.SetScoreText(score);
    }
}
