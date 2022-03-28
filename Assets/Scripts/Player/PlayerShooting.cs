using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;                      

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;                

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

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
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
        gunLine.SetPosition(0, transform.position);

        // Set posisi ray shoot dan direction.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

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

            // Set line position ke posisi hit.
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            // Set line position ke posisi ray shoot + range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}