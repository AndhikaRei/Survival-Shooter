using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.2f;        
    public float range = 100f;

    // Diagonal shoot weapon upgrade.
    public int diagonalUpgrade = 0;         

    // Attack Speed.
    public int aspdUpgrade = 0;  

    public float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
    public float effectsDisplayTime = 0.2f;                

    void Awake()
    {
        // Get Mask.
        shootableMask = LayerMask.GetMask("Shootable");
        
        // Mendapatkan reference komponen.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets / (aspdUpgrade + 1))
        {
            Shoot();
        }

        if (timer >= (timeBetweenBullets * effectsDisplayTime) / (aspdUpgrade + 1))
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void Shoot()
    {
        timer = 0f;

        // Play audio.
        gunAudio.Play();

        // Enable light.
        gunLight.enabled = true;

        // Play gun particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable line renderer dan set posisi awal.
        gunLine.enabled = true;

        // Initialize number of point in line and initialize empty point array.
        gunLine.positionCount = 2 + (diagonalUpgrade*4);
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i <= diagonalUpgrade ; i++)
        {
            // Set posisi ray shoot dan direction.
            shootRay.origin = transform.position;
            
            // Get number of lines shoot, if diagonal shoot, then shoot 2 lines else shoot 1 lines. 
            int numLines = i == 0 ? 1 : 2;

            for (int j = -1; j < numLines; j+=2) 
            {
                // Perhatikan apabila i==0 maka sudut quaternion akan selalu (0,0,0)
                // Jika i != 0 maka sudut quaternion akan berubah sesuai i (0, 30*i*j, 0), J \
                // antara 1 atau -1.
                shootRay.direction = Quaternion.Euler(0, i*j*10, 0) * transform.forward;

                // Lakukan raycast jika menemukan object yang dapat di shoot.
                if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
                {
                    // Lakukan raycast hit untuk mendapatkan informasi enemy.
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        // Berikan damage ke enemy.
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                    }

                    // Simpan point origin tembakan dan point lokasi tembakan.
                    points.Add(transform.position);
                    points.Add(shootHit.point);
                }
                else
                {
                    // Simpan point origin tembakan dan point lokasi tembakan.
                    points.Add(transform.position);
                    points.Add(shootRay.origin + shootRay.direction * range);
                }   
            }
             
        }

        // Set posisi line renderer.
        gunLine.SetPositions(points.ToArray());
    }
}