using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    
    public GameObject powerOrbPrefab;
    public GameObject speedOrbPrefab;
    public GameObject healthOrbPrefab;
    
    float timer;
    float spawnInterval = 5f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval){
            // Reset Timer
            timer = 0f; 

            // Random Orb
            int spawnOrb = Random.Range(0,3);

            // Random Position
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-20,21), 1, Random.Range(-20,21) );
            if(spawnOrb == 1){
                Instantiate(powerOrbPrefab, randomSpawnPosition, Quaternion.identity);
                Debug.Log("An Power Orb Spawned on" + randomSpawnPosition.ToString());
            } else if(spawnOrb == 2){
                Instantiate(speedOrbPrefab, randomSpawnPosition, Quaternion.identity);
                Debug.Log("An Speed Orb Spawned on" + randomSpawnPosition.ToString());
            } else{
                Instantiate(healthOrbPrefab, randomSpawnPosition, Quaternion.identity);
                Debug.Log("An Health Orb Spawned on" + randomSpawnPosition.ToString());
            }
            
        }
    }
}
