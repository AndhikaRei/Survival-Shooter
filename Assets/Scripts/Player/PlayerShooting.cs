using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public int maxDamagePerShot = 100;                     
    public float timeBetweenBullets = 0.2f;        
    public float range = 100f;

    public Camera fpsCamera;

    // Diagonal shoot weapon upgrade.
    public int diagonalUpgrade = 0;         

    // Attack Speed.
    public int aspdUpgrade = 0;

    // Multi shot
    public int penetUpgrade = 0;

    public float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;
    RaycastHit[] hitPoints;
    bool reachMaxRange;
    int shootableMask;                             
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
    public float effectsDisplayTime = 0.2f;                
    Text powerText;
    void Awake()
    {
        // Get Mask.
        shootableMask = LayerMask.GetMask("Shootable");
        
        // Mendapatkan reference komponen.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        // Get the powerText from hudcanvas.
        powerText = GameObject.Find("Power").GetComponent<Text>();
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

       powerText.text = damagePerShot.ToString();
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
            
            
            // Get number of lines shoot, if diagonal shoot, then shoot 2 lines else shoot 1 lines. 
            int numLines = i == 0 ? 1 : 2;

            for (int j = -1; j < numLines; j+=2) 
            {
                // Perhatikan apabila i==0 maka sudut quaternion akan selalu (0,0,0)
                // Jika i != 0 maka sudut quaternion akan berubah sesuai i (0, 30*i*j, 0), J \
                // antara 1 atau -1.
                if (CameraModeManager.cameraMode == CameraMode.TPS) 
                {
                    shootRay.origin = transform.position;
                    shootRay.direction = Quaternion.Euler(0, i*j*10, 0) * transform.forward;
                }
                else if (CameraModeManager.cameraMode == CameraMode.FPS)
                {
                    Vector3 point = Quaternion.Euler(0, i * j * 10, 0) * new Vector3(0.5f, 0.5f, 0.5f);
                    shootRay = fpsCamera.ViewportPointToRay(point);
                }

                points.Add(transform.position);

                // Lakukan raycast hit untuk mendapatkan informasi enemy.
                hitPoints = Physics.RaycastAll(shootRay, range, shootableMask).OrderBy(h => h.distance).ToArray();
                hitPoints = hitPoints.GroupBy(h => h.transform).Select(g => g.First()).ToArray();

                int hitCount;

                if (hitPoints.Length < penetUpgrade + 1)
                {
                    reachMaxRange = true;
                    hitCount = hitPoints.Length;
                } 
                else
                {
                    reachMaxRange = false;
                    hitCount = penetUpgrade + 1;
                }

                for (int k = 0; k < hitCount; k++)
                {
                    EnemyHealth enemyHealth = hitPoints[k].collider.GetComponent<EnemyHealth>();
                    
                    if (enemyHealth != null)
                    {
                        // Berikan damage ke enemy.
                        enemyHealth.TakeDamage(damagePerShot, hitPoints[k].point);
                    }
                }

                if (reachMaxRange)
                {
                    points.Add(shootRay.origin + shootRay.direction * range);
                }
                else
                {
                    points.Add(hitPoints[hitCount-1].point);
                }
            }
             
        }

        // Set posisi line renderer.
        gunLine.SetPositions(points.ToArray());
    }

    public void PickupOrb(int amount)
    {
        // Menambahkan damagePerShot dengan amount.
        if(damagePerShot + amount >= maxDamagePerShot){
            damagePerShot = maxDamagePerShot;
        }else{
           damagePerShot += amount;
        }
        
        // Update power data.
        powerText.text = damagePerShot.ToString();
    }
}