using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Animator shooterAnimator;
    public GameObject bulletPrefab;

    //shooting stuff
    private float spaceHeldTime = 0f;
    public float minTimeForSmallBeam = 0.4f;
    public float minTimeForBigBeam = 1f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isPlaying)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if(spaceHeldTime > minTimeForSmallBeam)
                shooterAnimator.SetBool("chargeShot", true);
            spaceHeldTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            shooterAnimator.SetBool("chargeShot", false);
            if (spaceHeldTime < minTimeForSmallBeam)
            {
                shooterAnimator.SetTrigger("smallShot");
                Bullet smallBullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
                smallBullet.setType(1);
            }
            else if (spaceHeldTime < minTimeForBigBeam)
            {
                shooterAnimator.SetTrigger("beamShot");
                Bullet smallBeam = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
                smallBeam.setType(2);
            }
            else
            {
                shooterAnimator.SetTrigger("beamShot");
                Bullet bigBeam = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
                bigBeam.setType(3);
            }
            spaceHeldTime = 0f;
        }
    }
}
