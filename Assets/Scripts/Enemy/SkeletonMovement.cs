using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    Rigidbody rb;


    private void Awake ()
    {
        // Cari game object dengan tag player.
        player = GameObject.FindGameObjectWithTag ("Player").transform;

        // Mendapatkan reference component.
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        rb = GetComponent <Rigidbody>();

    }


    void Update ()
    {
        // Mengganti direction dari skeleton ke arah player
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0){
            Vector3 direction = player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }     
    }
}
