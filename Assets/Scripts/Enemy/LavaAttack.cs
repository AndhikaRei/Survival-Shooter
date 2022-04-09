using UnityEngine;
using System.Collections;

public class LavaAttack : MonoBehaviour
{
    public int attackDamage = 15;
    public GameObject explosionEffect;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    bool isExploded = false;
    float attackRange = 2f;
    GameObject explosion;


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

    void Update ()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        
        if(playerDistance <= attackRange){
            playerInRange = true;
        }else{
            playerInRange = false;
        }

        if(playerInRange && enemyHealth.currentHealth > 0 && !isExploded)
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
        Instantiate(explosionEffect, transform.position, transform.rotation);
        // Taking damage.
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
        isExploded = true;
        enemyHealth.SelfDestruction();
        enemyHealth.StartSinking();
    }
}
