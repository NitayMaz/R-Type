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
    private Rigidbody2D mapRb;
    private GameObject player;
    private PlayerController playerController;
    private Camera gameCamera;
    private AudioSource audioSource;
    public AudioClip StartGameSound;
    public int score = 0;
    public float mapMoveSpeed = 1.1f;

    public List<GameObject> enemies;
    private float maxCameraX;
    public float DistanceFromCameraToSpawnEnemy = 1f;
    private int nextEnemyIndex = 0;

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
        mapRb = map.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        gameCamera = Camera.main;
        maxCameraX = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;
        //sort enemies by x position so that we always know the next enemy to spawn
        enemies = enemies.OrderBy(obj => obj.transform.position.x).ToList();
        StartCoroutine(StartNewGame());
    }
    IEnumerator StartNewGame()
    {
        audioSource.PlayOneShot(StartGameSound);
        //wait for animation to finish before starting the game
        if (playStartAnimation)
            yield return StartCoroutine(playerController.PlayOpeningAnimation());
        Debug.Log("Game started");
        isPlaying = true;
    }
    

    private void Update()
    {
        if (isPlaying)
        {
            HandleSpawnEnemies();
        }
    }

    private void HandleSpawnEnemies()
    {

        if (nextEnemyIndex < enemies.Count &&
                enemies[nextEnemyIndex].transform.position.x < maxCameraX + DistanceFromCameraToSpawnEnemy)
        {
            enemies[nextEnemyIndex].SetActive(true);
            nextEnemyIndex++;
        }
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            MoveMap();
        }
    }

    private void MoveMap()
    {
        mapRb.MovePosition(mapRb.position + Vector2.left * mapMoveSpeed * Time.fixedDeltaTime);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIHandler.instance.SetScoreText(score);
    }
}
