using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    GameObject player;
    GameObject gunBarrelEnd;
    int extraHealth = 50;
    float extraSpeed = 0.5f;
    int extraPower = 10;
    void Awake()
    {
        // Mencari game object dengan tag player.
        player = GameObject.FindGameObjectWithTag ("Player");
        gunBarrelEnd = GameObject.Find("GunBarrelEnd");

        // Mendapatkan komponen playerHealth.
        playerHealth = player.GetComponent <PlayerHealth> ();
        playerMovement = player.GetComponent <PlayerMovement> ();
        playerShooting = gunBarrelEnd.GetComponent <PlayerShooting> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
        {
            Pickup();    
        }
    }

    void Pickup(){
        Debug.Log(gameObject.tag + " Orb has being picked up!");

        // Add stats to player
        if (gameObject.CompareTag("Health"))
        {
            playerHealth.PickupOrb(extraHealth);
        } else if (gameObject.CompareTag("Power"))
        {
            playerShooting.PickupOrb(extraPower);
        } else
        {
            playerMovement.PickupOrb(extraSpeed);
        }

        // Destroy orb after being picked up
        Destroy(gameObject);
    }
}