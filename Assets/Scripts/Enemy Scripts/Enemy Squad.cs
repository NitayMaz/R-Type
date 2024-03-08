using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad : MonoBehaviour
{
    public void checkShouldDestroy()
    {
        //the last child called this function when it's supposed to be destroyed, meaning there's no one left that should be destroyed now
        if (transform.childCount == 1)
        {
            Destroy(gameObject);
        }
    }
}
