using UnityEngine;
using System.Collections;

public class SkeletonAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 5;
    public GameObject fireballEffect;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    float attackRange = 10f;


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
        timer += Time.deltaTime;
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        
        if(playerDistance <= attackRange){
            playerInRange = true;
        }else{
            playerInRange = false;
        }
        anim.SetBool("PlayerInRange", playerInRange);

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
        GameObject effect = Instantiate (fireballEffect, transform.position, transform.rotation);
        Destroy (effect, 1.0f);
        // Taking damage.
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
