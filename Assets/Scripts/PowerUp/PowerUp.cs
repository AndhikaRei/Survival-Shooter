using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    AudioSource pickupAudio;

    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    GameObject player;
    GameObject gunBarrelEnd;

    int extraHealth = 50;
    float extraSpeed = 0.5f;
    int extraPower = 10;
    float timer;
    float maxTime = 10f;

    float pickupDistance = 4f;

    void Awake()
    {
        pickupAudio = GetComponent<AudioSource>();
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
        timer += Time.deltaTime;
        if(timer >= maxTime){
            Debug.Log(gameObject.tag + " Orb has being destroyed!");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
        {
            // Check player distance with current power up.
            float distance = Vector3.Distance(player.transform.position, transform.position);
            //  If distance is shorter than pickupDistance, then power up is picked up.
            if (distance < pickupDistance) {
                Pickup();
            }
        }
    }

    void Pickup(){
        Debug.Log(gameObject.tag + " Orb has being picked up!");

        transform.position += Vector3.down * 5;

        pickupAudio.Play();
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
        Destroy(gameObject, pickupAudio.clip.length);
    }
}
