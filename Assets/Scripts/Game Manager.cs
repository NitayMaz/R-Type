using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioClip startGameMusic;
    public AudioClip gameMusicLoop;
    public AudioClip gameOverSound;
    public int score = 0;
    private int highScore;
    public List<float> checkpointsX = new List<float>();
    private int lastCheckpointIndex = 0;
    private Vector3 startPlayerCameraOffset;
    public int difficulty;


    //isPlaying is used to stop the game when the player dies, I use it instead of time.timeScale because the latter wouldn't stop objects(mainly bullets) from being created
    public bool isPlaying = false;
    private int livesRemaining = 2; // for a total of 3 lives



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
        audioSource.PlayOneShot(startGameMusic); // the level isn't long enough for this one to need to loop
        startPlayerCameraOffset = player.transform.position - gameCamera.transform.position;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        difficulty = PlayerPrefs.GetInt("Difficulty", 2);
        UIHandler.instance.SetHighScoreText(highScore);
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

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIHandler.instance.SetScoreText(score);
    }

    private void MoveCamera()
    {
        gameCamera.transform.position += Vector3.right * Time.fixedDeltaTime * difficulty;
        if (lastCheckpointIndex+1< checkpointsX.Count() && gameCamera.transform.position.x > checkpointsX[lastCheckpointIndex+1])
            AdvanceCheckpoint();
    }

    private void AdvanceCheckpoint()
    {
        lastCheckpointIndex++;
        EnemySpawner.instance.HitCheckPoint();
    }

    private void DeleteObjectsFromScene()
    {
        // delete all enemies, bullets and enemy bullets for reseting from checkpoint
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesInScene)
        {
            Destroy(enemy);
        }
        GameObject[] bulletsInScene = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject enemy in bulletsInScene)
        {
            Destroy(enemy);
        }
        GameObject[] enemyBulletsInScene = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject enemy in enemyBulletsInScene)
        {
            Destroy(enemy);
        }
    }


    public void RestartFromCheckPoint()
    {
        //deleta objects
        DeleteObjectsFromScene();
        //reset camera to last transform
        gameCamera.transform.position = new Vector3(checkpointsX[lastCheckpointIndex], gameCamera.transform.position.y, gameCamera.transform.position.z);
        //shift back enemy spawner
        EnemySpawner.instance.ResetToCheckPoint();
        Vector2 playerPosition = gameCamera.transform.position + startPlayerCameraOffset;
        playerController.RespawnPlayer(playerPosition);
        audioSource.PlayOneShot(gameMusicLoop);

    }

    public IEnumerator PlayerDied()
    {
        audioSource.Stop(); // stop background music
        StartCoroutine(playerController.PlayDeathAnimation());
        isPlaying = false;
        if (livesRemaining > 0)
        {
            livesRemaining--;
            UIHandler.instance.RemoveLifeIcon(livesRemaining);
            // wait a bit to let player see what killed him and for death animation to finish
            yield return new WaitForSeconds(1.5f);
            RestartFromCheckPoint();
            // wait before restarting the game
            yield return new WaitForSeconds(2);
            isPlaying = true;
        }
        else
        {
            audioSource.PlayOneShot(gameOverSound);
            if(score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                PlayerPrefs.Save();
            }
            yield return new WaitForSeconds(gameOverSound.length); //let the sound effect finish then get out
            SceneManager.LoadScene("Menu");
            
        }
    }
}
