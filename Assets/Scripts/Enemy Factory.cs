using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Swarm
}
public class EnemyFactory : MonoBehaviour
{
    public GameObject SwarmPrefab;
    public Enemy CreateEnemy(EnemyType type, Vector3 location)
    {
        switch (type)
        {
            case EnemyType.Swarm:
                Instantiate(SwarmPrefab,location,Quaternion.identity);
                return null;
            default:
                Debug.Log($"Enemy type {type} not found");
                return null;
        }
    }
}
