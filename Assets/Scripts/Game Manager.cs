using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyFactory enemyFactory;

    public int score = 0;

    //isPlaying is used to stop the game when the player dies, I use it instead of time.timeScale because the latter wouldn't stop objects(mainly bullets) from being created
    public bool isPlaying = true;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemyFactory.CreateEnemy(EnemyType.CentipedeDownLeft, new Vector3(10, 1, 0));
    }

}
