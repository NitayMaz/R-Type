using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RemoveWhenOutsideScreen : MonoBehaviour
{
    public float leftEdgeBuffer = 0.05f, rightEdgeBuffer = 0.5f, topEdgeBuffer = 0.1f, bottomEdgeBuffer =0.1f;
    public EnemySquad squad;
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        //no need to check for the bottom edge, since the floor destroys everything it touches
        if ( viewportPosition.y < -bottomEdgeBuffer || viewportPosition.y > 1 + topEdgeBuffer ||
            viewportPosition.x < -leftEdgeBuffer || viewportPosition.x > 1+rightEdgeBuffer)
        {
            if (squad != null)
            {
                squad.checkShouldDestroy();
            }
            Destroy(gameObject);
        }
    }
}
