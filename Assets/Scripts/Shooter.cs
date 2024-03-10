using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Animator shooterAnimator;
    public GameObject bulletPrefab;

    //shooting stuff
    private float spaceHeldTime = 0f;
    public float minTimeForSmallBeam = 0.4f;
    public float minTimeForBigBeam = 1f;
    private AudioSource audioSource;
    public AudioClip bulletShotSound;
    public AudioClip chargeSound;
    public AudioClip beamShotSound;
    // Start is called before the first frame update


    private void Awake()
    {
        shooterAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPlaying)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeShot();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseShot();
        }
    }

    private void ChargeShot()
    {
        if (spaceHeldTime > minTimeForSmallBeam / 2)
        {
            shooterAnimator.SetBool("chargeShot", true);
            audioSource.clip = chargeSound;
            audioSource.loop = true;
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.5f;
                audioSource.Play();
            }
        }
        spaceHeldTime += Time.deltaTime;
        UIHandler.instance.SetBeamSliderValue(spaceHeldTime / minTimeForBigBeam);
    }

    private void ReleaseShot()
    {
        shooterAnimator.SetBool("chargeShot", false);
        audioSource.Stop();
        audioSource.loop = false;
        if (spaceHeldTime < minTimeForSmallBeam)
        {
            audioSource.volume = 1f;
            audioSource.PlayOneShot(bulletShotSound);
            shooterAnimator.SetTrigger("smallShot");
            Bullet smallBullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            smallBullet.setType(1);
        }
        else //beam
        {
            audioSource.volume = 1f;
            shooterAnimator.SetTrigger("beamShot");
            audioSource.PlayOneShot(beamShotSound);
            if (spaceHeldTime < minTimeForBigBeam)
            {
                Bullet smallBeam = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
                smallBeam.setType(2);
            }
            else
            {
                Bullet bigBeam = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
                bigBeam.setType(3);
            }
        }
        spaceHeldTime = 0f;
        UIHandler.instance.SetBeamSliderValue(spaceHeldTime / minTimeForBigBeam);
    }

    public void ResetShooter()
    {
        shooterAnimator.SetBool("chargeShot", false);
        spaceHeldTime = 0f;
        audioSource.Stop();
    }
}
