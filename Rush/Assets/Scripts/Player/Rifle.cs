using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Settings")]
    public GameObject bullet;
    public Transform shootpoint;
    public int ammo;
    public float speed;
    private Transform destruct;
    private bool shooting;
    private float shootspeed = 0.125f;
    [Header("Recoil Settings")]
    public float recoil, recoilAddSpeed, recoilSubSpeed, spreadMultiplier;
    [Header("Sound Settings")]
    public AudioSource rifleSound;
    public AudioClip shot, reload;
    public float shotVolume = 0.2f;
    public ParticleSystem flash;
    public Light muzzleFlash;
    public float flashIntensity;
    public float flashDecayRate;
    private Vector3 shootDir;
    private Animator anim;

    void Awake()
    {
        destruct = GameObject.Find("Destructables").transform;
        anim = this.GetComponent<Animator>();
    }

    private float tick;
    void FixedUpdate()
    {
        if (ammo <= 0){shooting = false;}
        bool canShoot = ((anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Armature|Arms_FPS_Anim_Idle")||
        (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Armature|Arms_FPS_Anim_Walk")||
        (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Armature|Arms_FPS_Anim_Run")||
        (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Armature|Arms_FPS_Anim_Shoot"));
        anim.SetBool("Shooting", shooting);
        tick += Time.deltaTime;
        if((shooting)&&(tick>shootspeed)&&(canShoot))
        {
            tick = 0;
            recoil += recoilAddSpeed;
            if((ammo>0)&&(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Armature|Arms_FPS_Anim_Run")){
                Shoot();
            }
        }
        if((!shooting)&&(recoil > recoilSubSpeed)){
            recoil -= recoilSubSpeed;
        }
        else if ((!shooting)&&(recoil <= recoilSubSpeed)){
            recoil = 0;
        }
        if(muzzleFlash.intensity >= flashDecayRate){
            muzzleFlash.intensity -= flashDecayRate;
        }
        else{
            muzzleFlash.intensity = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            Debug.Break();
        }
        if(Input.GetMouseButtonDown(0)){
            tick = 15f;
            shooting = true;
        }
        if (Input.GetMouseButtonUp(0)){
            shooting = false;
        }
        if ((Input.GetKeyDown(KeyCode.R))&&(!shooting)&&(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Armature|Arms_FPS_Anim_Reload_Fast")&&(ammo!=25)){
            anim.SetTrigger("Reload");
            rifleSound.pitch = .85f + (Random.Range(1,10)/100f * 1.5f);
            rifleSound.PlayOneShot(reload, 0.5f);
            ammo = 25;
        }
    }

    void Shoot()
    {
        float ang = recoil * spreadMultiplier;
        float randomX = Random.Range(-ang, ang);
        float randomY = Random.Range(-ang, ang);
        float randomZ = Random.Range(-ang, ang);
        Quaternion newRotation = Quaternion.Euler(randomX, randomY, randomZ);
        GameObject tempBullet = Instantiate(bullet, shootpoint.position, Quaternion.identity);
        shootpoint.rotation = transform.rotation * newRotation;
        tempBullet.transform.SetParent(destruct);
        Rigidbody rb = tempBullet.GetComponent<Rigidbody>();
        rb.AddForce(shootpoint.transform.forward * speed);
        ammo -= 1;
        //flash.Play();
        rifleSound.pitch = 1.2f + (Random.Range(1,16)/100f * 1.5f);
        rifleSound.PlayOneShot(shot, shotVolume);
        muzzleFlash.intensity = flashIntensity;
    }
}
