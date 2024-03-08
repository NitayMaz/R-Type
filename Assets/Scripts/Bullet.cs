using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //TODO: make this an enum ffs
    public int type; // 1 is a small bullet, 2 is a small charged beam, 3 is a big charged beam

    private Rigidbody2D rb;
    private Animator bulletAnimator;
    private BoxCollider2D bulletCollider;

    public float speed = 10f;
    public Vector2 smallBeamColliderDimensions;
    public Vector2 largeBeamColliderDimensions;
    public float xOffsetSmallBeam = 0.5f;
    public float xOffsetBigBeam = 1.5f;
    private int simpleBulletDamage = 1;
    private int smallBeamDamage = 3;
    private int largeBeamDamage = 3;
    public int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletAnimator = GetComponent<Animator>();
        bulletCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.instance.isPlaying)
        {
            return;
        }
        rb.MovePosition(rb.position + Vector2.right * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            if(type==1 || type == 2)
                Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Map" || collision.gameObject.tag == "Blocker")
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
