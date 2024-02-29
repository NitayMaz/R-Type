using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Swarm,
    CentipedeUpLeft,
    CentipedeDownLeft
}
public class EnemyFactory : MonoBehaviour
{
    public GameObject SwarmPrefab;
    public GameObject CentipedeUpLeftPrefab;
    public GameObject CentipedeDownLeftPrefab;
    public Enemy CreateEnemy(EnemyType type, Vector3 location)
    {
        switch (type)
        {
            case EnemyType.Swarm:
                GameObject swarm = Instantiate(SwarmPrefab,location,Quaternion.identity);
                return null;
            case EnemyType.CentipedeUpLeft:
                GameObject centipedeUp = Instantiate(CentipedeUpLeftPrefab, location, Quaternion.identity);
                CentipedePiece[] upPieces = centipedeUp.GetComponentsInChildren<CentipedePiece>();
                foreach(CentipedePiece piece in upPieces)
                        piece.SetSpeed((new Vector3(-1, 1, 0)).normalized);
                return null;
            case EnemyType.CentipedeDownLeft:
                GameObject centipedeDown = Instantiate(CentipedeDownLeftPrefab, location, Quaternion.identity);
                CentipedePiece[] downPieces = centipedeDown.GetComponentsInChildren<CentipedePiece>();
                foreach (CentipedePiece piece in downPieces)
                        piece.SetSpeed((new Vector3(-1, -1, 0)).normalized);

                return null;
            default:
                Debug.Log($"Enemy type {type} not found");
                return null;
        }
    }
}
