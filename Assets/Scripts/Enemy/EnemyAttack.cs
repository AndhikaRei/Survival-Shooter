using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        // Mencari game object dengan tag player.
        player = GameObject.FindGameObjectWithTag ("Player");
        
        // Mendapatkan komponen playerHealth.
        playerHealth = player.GetComponent <PlayerHealth> ();
        
        // Mendapatkan komponen animator.
        anim = GetComponent <Animator> ();

        // Mendapatkan komponen enemyHealth.
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Callback ketika ada suatu object masuk kedalam trigger.
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;
        }
    }

    // Callback ketika ada suatu object keluar dari trigger.
    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        // Mentrigger animasi PlayerDead jika darah player kurang dari sama dengan 0.
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }


    void Attack ()
    {
        // Reset Timer.
        timer = 0f;

        // Taking damage.
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
