using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //TODO: make this an enum ffs
    public int type; // 1 is a small bullet, 2 is a small charged beam, 3 is a big charged beam

    public float speed = 10f;
    public Animator bulletAnimator;
    public BoxCollider2D bulletCollider;
    public Vector2 smallBeamColliderDimensions;
    public Vector2 largeBeamColliderDimensions;
    
    public float xOffsetSmallBeam = 0.5f;
    public float xOffsetBigBeam = 1.5f;
    public int simpleBulletDamage = 1;
    public int smallBeamDamage = 1;
    public int largeBeamDamage = 3;
    public int damage;


    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isPlaying)
        {
            return;
        }
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.health -= damage;
            if (enemy.health <= 0)
            {
                enemy.Die();
            }
            if(type==1)
                Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Map")
        {
            Destroy(gameObject);
        }
    }

    public void setType(int t)
    {
        type = t;
        damage = simpleBulletDamage;
        if(type==2)
        {
            bulletAnimator.SetBool("smallBeam", true);
            transform.position += Vector3.right * xOffsetSmallBeam;
            bulletCollider.size = smallBeamColliderDimensions;
            damage = smallBeamDamage;
        }
        if(type==3)
        {
            bulletAnimator.SetBool("largeBeam", true);
            transform.position += Vector3.right * xOffsetBigBeam;
            bulletCollider.size = largeBeamColliderDimensions;
            damage = largeBeamDamage;
        }
    }
}
