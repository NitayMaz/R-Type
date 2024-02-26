using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int type; // 1 is a small bullet, 2 is a small charged beam, 3 is a big charged beam

    public float speed = 10f;
    public Animator bulletAnimator;
    
    public float xOffsetSmallBeam = 0.5f;
    public float xOffsetBigBeam = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        
    }

    public void setType(int t)
    {
        type = t;
        if(t==2)
        {
            bulletAnimator.SetBool("smallBeam", true);
            transform.position += Vector3.right * xOffsetSmallBeam;
        }
        if(t==3)
        {
            bulletAnimator.SetBool("largeBeam", true);
            transform.position += Vector3.right * xOffsetBigBeam;
        }
    }
}
