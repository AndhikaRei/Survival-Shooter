using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    // Enemy manager.
    public EnemyManager enemyManager;
    // Game mode manager.
    public GameModeManager gameModeManager;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        // Mendapatkan reference komponen.
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        gameModeManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

        // Inisialisasi currentHealth.
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // Check jika enemy telah mati dan belum keluar dari scene.
        if (isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        // Check jika enemy telah mati.
        if (isDead)
            return;

        // Play audio.
        enemyAudio.Play ();

        // Kurangi currentHealth.
        currentHealth -= amount;

        // Ganti Posisi Particle
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        // Dead jika currentHealth kurang dari 0.
        if (currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        // Set isDead.
        isDead = true;

        // Set CapsuleCollider ke trigger.
        capsuleCollider.isTrigger = true;

        // Play animasi dead.
        anim.SetTrigger ("Dead");

        // Play audio dead.
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

        // Add the number of killed enemy if the game is wave mode.
        if (GameModeManager.gameMode == GameMode.Wave)
        {
            enemyManager.killedEnemyAmount++;
        }
    }


    public void StartSinking ()
    {
        // Disable navsmesh.
        GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
        // Set rigidbody ke kinematic.
        GetComponent<Rigidbody> ().isKinematic = true;
        isSinking = true;
        
        // Update the score when the game is not zen mode.
        if (GameModeManager.gameMode != GameMode.Zen)
        {
            ScoreManager.score += scoreValue;
        }
        Destroy (gameObject, 2f);
    }
}
