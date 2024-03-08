using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization;



[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector GUI

        EnemySpawner spawner = (EnemySpawner)target; // Reference to the EnemySpawner script

        // this one spawns all enemies at once for testing purposes
        if (GUILayout.Button("Spawn All Enemies"))
        {
            spawner.SpawnAllEnemies();
        }
        //this one takes existing enemy objects on the scene and adds them to the list
        if (GUILayout.Button("Populate From Selected GameObjects"))
        {
            PopulateFromSelectedObjects(spawner);
        }
    }

    private void PopulateFromSelectedObjects(EnemySpawner spawner)
    {
        spawner.enemyLocations.Clear(); // Clear existing list

        foreach (GameObject obj in Selection.gameObjects)
        {
            EnemySpawner.EnemyLocation newLocation = new EnemySpawner.EnemyLocation
            {
                spawnLocation = obj.transform.position,
                enemyType = DetermineEnemyType(obj)
            };

            spawner.enemyLocations.Add(newLocation);
        }

        // Mark the spawner as dirty to ensure the changes are saved
        EditorUtility.SetDirty(spawner);
    }

    // Implement this based on your criteria
    private EnemySpawner.Enemy DetermineEnemyType(GameObject obj)
    {
        if(obj.name.Contains("Swarm"))
        {
            return EnemySpawner.Enemy.Swarm;
        }
        else if(obj.name.Contains("Centipede"))
        {
            return EnemySpawner.Enemy.Centipede;
        }
        else if(obj.name.Contains("RobotSquad"))
        {
            return EnemySpawner.Enemy.RobotSquad;
        }
        else if(obj.name.Contains("mecha"))
        {
            return EnemySpawner.Enemy.Mecha;
        }
        else if(obj.name.Contains("Floor Turrets"))
        {
            return EnemySpawner.Enemy.FloorTurrets;
        }
        else if(obj.name.Contains("Ceil Turrets"))
        {
            return EnemySpawner.Enemy.CeilTurrets;
        }
        else if(obj.name.Contains("ShooterCircle"))
        {
            return EnemySpawner.Enemy.ShooterCircle;
        }
        else
        {
            Debug.LogAssertion("Unknown enemy type: " + obj.name);
            return EnemySpawner.Enemy.Swarm;
        }
    }



}
