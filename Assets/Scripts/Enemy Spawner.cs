using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum Enemy
    {
        Swarm,
        Centipede,
        RobotSquad,
        Mecha,
        FloorTurrets,
        CeilTurrets,
        ShooterCircle
    }

    [System.Serializable]
    public class EnemyLocation
    {
        public Vector3 spawnLocation;
        public Enemy enemyType;
    }

    public static EnemySpawner instance;

    public List<EnemyLocation> enemyLocations = new List<EnemyLocation>();
    private int nextEnemyIndex = 0;
    private int nextEnemyIndexAtCheckpoint = 0;
    public float DistanceFromCameraToSpawnEnemy = 1f;
    public bool spawnEnemies = true;

    public GameObject swarmPrefab;
    public GameObject centipedePrefab;
    public GameObject robotSquadPrefab;
    public GameObject mechaPrefab;
    public GameObject floorTurretsPrefab;
    public GameObject ceilTurretsPrefab;
    public GameObject shooterCirclePrefab;
    private Camera mainCamera;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Singleton violation");
        }
        instance = this;
    }

    void Start()
    {
        // Populate the list with your enemy locations and types

        // Sort the list based on spawn positions
        enemyLocations.Sort((a, b) => (a.spawnLocation.x).CompareTo(b.spawnLocation.x));
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            HandleSpawnEnemies();
        }
    }

    public void HandleSpawnEnemies()
    {
        if(!spawnEnemies)
        {
            return;
        }
        float maxCameraX = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;
        if (nextEnemyIndex < enemyLocations.Count &&
                enemyLocations[nextEnemyIndex].spawnLocation.x < maxCameraX + DistanceFromCameraToSpawnEnemy)
        {
            EnemySpawnFactory(enemyLocations[nextEnemyIndex]);
            nextEnemyIndex++;
        }
    }

    public void HitCheckPoint()
    {
        nextEnemyIndexAtCheckpoint = nextEnemyIndex;
    }
    public void ResetToCheckPoint()
    {
        nextEnemyIndex = nextEnemyIndexAtCheckpoint;
    }

    private void EnemySpawnFactory(EnemyLocation enemyLocation)
    {
        switch (enemyLocation.enemyType)
        {
            case Enemy.Swarm:
                Instantiate(swarmPrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.Centipede:
                Instantiate(centipedePrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.RobotSquad:
                Instantiate(robotSquadPrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.Mecha:
                Instantiate(mechaPrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.FloorTurrets:
                Instantiate(floorTurretsPrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.CeilTurrets:
                Instantiate(ceilTurretsPrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
            case Enemy.ShooterCircle:
                Instantiate(shooterCirclePrefab, enemyLocation.spawnLocation, Quaternion.identity);
                break;
        }
    }

    // for testing purposes only
    public void SpawnAllEnemies()
    {
        foreach (EnemyLocation location in enemyLocations)
        {
            EnemySpawnFactory(location);
        }
    }

}
