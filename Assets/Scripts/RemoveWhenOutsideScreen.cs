using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RemoveWhenOutsideScreen : MonoBehaviour
{
    public float leftEdgeBuffer = 0.05f, rightEdgeBuffer = 0.3f, topEdgeBuffer = 0.05f;
    private Camera mainCamera;
    public GameObject indicator;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        //no need to check for the bottom edge, since the floor destroys everything it touches
        if (viewportPosition.y > 1+topEdgeBuffer ||
            viewportPosition.x < -leftEdgeBuffer || viewportPosition.x > 1+rightEdgeBuffer)
        {
            Destroy(gameObject);
        }
    }
}
