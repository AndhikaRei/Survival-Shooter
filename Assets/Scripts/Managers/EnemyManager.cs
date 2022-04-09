using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public float spawnTime = 1f;


    [SerializeField]
    MonoBehaviour factory;
    IFactory Factory {
        get {
            return factory as IFactory;
        }
    }

    // Game mode manager.
    public GameModeManager gameModeManager;

    public WeaponUpgradeManager weaponUpgradeManager;

    // In the wave mode, each wave have a maximum 'weight' of enemy spawned, and in each wave
    // there are enemy pool that can be summoned randomly. Example wave 1 have 5 weight and we can
    // only summmon zombear with weight 1, it means we can only summon 5 zombear in this wave. Enemy
    // are summoned randomly.

    // Enemy tag.
    public static readonly List<int> enemyTag = new List<int>() { 
        0, // Zombear
        1, // Zombunny
        2, // Zomhellephant
        3, // Skeleton
        4, // Bomber
        5, // Boss
    };
    
    // Weight of each enemy.
    public static readonly List<int> enemyWeight = new List<int>() {
        1,
        1,
        2,
        3,
        2,
        4,
    };

    // Max wave.
    public const int maxWave = 10;

    // Declare pool of enemy.
    public static readonly List<List<int>> enemyPool = new List<List<int>>() {
        new List<int>() { 0 }, // Wave 1
        new List<int>() { 0, 1 }, // Wave 2
        new List<int>() { 0, 1, 2 }, // Wave 3
        new List<int>() { 0, 1, 2 }, // Wave 4
        new List<int>() { 0, 1, 2, 3 }, // Wave 5
        new List<int>() { 0, 1, 2, 3 }, // Wave 6
        new List<int>() { 0, 1, 2, 3 }, // Wave 7
        new List<int>() { 0, 1, 2, 3, 4 }, // Wave 8
        new List<int>() { 0, 1, 2, 3, 4 }, // Wave 9
        new List<int>() { 0, 1, 2, 3, 4, 5 }, // Wave 10
    };
    
    // Wave weight.
    public static readonly List<int> waveWeight = new List<int>() {
        // Easy
        // 15 / 15, 
        // 30 / 15 ,
        // 45 / 15 ,
        // 75 / 15 ,
        // 90 / 15 ,
        // 105 / 15 ,
        // 135 / 15 ,
        // 150 / 15 ,
        // 165 / 15 ,
        // 195 / 15 ,

        // Hard
        15, 
        30 ,
        45 ,
        75 ,
        90 ,
        105 ,
        135 ,
        150 ,
        165 ,
        195 ,
    };

    // Current wave.
    public int currentWave = 0;
    // Current weigth.
    public int currentWeight = 0;
    // Spawned enemy tag.
    public List<int> spawnedEnemyTag = new List<int>();
    // Spawned weight.
    public int spawnedWeight = 0;
    // Spawned enemy amount.
    public int spawnedEnemyAmount = 0;
    // Killed enemy amount.
    public int killedEnemyAmount = 0;
    // Player win condition
    public bool gameWon = false;

    void Awake()
    {
         // Get game mode manager object.
        gameModeManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();
        // Get weapon upgrade manager object.
        weaponUpgradeManager = GameObject.Find("WeaponUpgradeManager").GetComponent<WeaponUpgradeManager>();
    }

    void Start ()
    {
        // If the game is zen mode, spawn enemy randomly and repeat.
        if (GameModeManager.gameMode == GameMode.Zen)
        {
            Debug.Log("Zen mode");
            InvokeRepeating ("SpawnZen", spawnTime, spawnTime);
        }
        
        if (GameModeManager.gameMode == GameMode.Wave)
        {
            // If the game is wave mode then start the wave.
            Debug.Log("Wave mode");
            StartWave();
        }
    }

    void Update ()
    {
        // If the game is wave mode check some event.
        if (GameModeManager.gameMode == GameMode.Wave && currentWave != 0)
        {
            // If current spawnedEnemyTag is empty or player is dead then cancel the invoke.
            if (spawnedEnemyTag.Count == 0 || playerHealth.currentHealth <= 0)
            {
                CancelInvoke("SpawnWave");
            }

            // If the killed enemy amount is equal to the spawned enemy amount and wave has 
            // been started then start the next wave.
            if (killedEnemyAmount == spawnedEnemyAmount)
            {
                // If current finished wave is divisible by 3 then let player to upgrade weapon.
                if (currentWave % 3 == 0)
                {
                    WeaponUpgradeManager.maxUpgrade++;
                }

                // Completed wave, start next wave
                if (currentWave < maxWave)
                {
                    NextWave();
                }

                // Completed last wave
                else if (currentWave == maxWave && !gameWon)
                {
                    StopWave();
                }
                
        
            }
        }

        // If the game is zen mode check some event.
        if (GameModeManager.gameMode == GameMode.Zen)
        {
            // Add 1 upgrade point every 30 second and make the spawning faster.
            if (Math.Floor(Math.Floor(ScoreManager.survival_time) / 30) > WeaponUpgradeManager.maxUpgrade )
            {
                WeaponUpgradeManager.maxUpgrade++;
                float divider = (float)(Math.Floor(Math.Floor(ScoreManager.survival_time) / 30));
                if (divider > 1) {
                    spawnTime = spawnTime / divider * (divider - 1);
                } else {
                    spawnTime = spawnTime / divider;
                }
                CancelInvoke("SpawnZen");
                InvokeRepeating ("SpawnZen", spawnTime, spawnTime);
            }
        }

    }

    // Spawn enemy in spawnedEnemyTag list.
    void SpawnWave() {
        
        // If spawned enemy tag is not empty, spawn enemy.
        if (spawnedEnemyTag.Count > 0) 
        {
            // Get the first enemy tag in the list and spawn the enemy.
            int enemyTag = spawnedEnemyTag[0];
            Factory.FactoryMethod(enemyTag);

            // Remove the first enemy tag in the list.
            spawnedEnemyTag.RemoveAt(0);
        }
        
    }

    // Spawn enemy with specified chance for each enemy.
    void SpawnZen ()
    {
        // Jika player telah mati, maka tidak melakukan spawn.
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }
        //  Default spawn enemy.
        int spawnEnemy = 0;

        // Randomize enemy with chance of each enemy.
        // Zombunny (tag-0) = 25%
        // Zombear (tag-1) = 25%
        // Zomhellephant (tag-2) = 15%
        // Skeleton (tag-3) = 15%
        // Bomber (tag-4) = 15%
        // Boss (tag-5) = 5%
        int chance = Random.Range (0, 100);
        // Spawn Zombunny.
        if (chance < 25)
        {
            spawnEnemy = 0;
        }
        // Spawn Zombear.
        else if (chance < 50)
        {
            spawnEnemy = 1;
        }
        // Spawn Zomhellephant.
        else if (chance < 65)
        {
            spawnEnemy = 2;
        }
        // Spawn Skeleton.
        else if (chance < 80)
        {
            spawnEnemy = 3;
        }
        // Spawn Bomber
        else if (chance < 95)
        {
            spawnEnemy = 4;
        }
        // Spawn boss
        else
        {
            spawnEnemy = 5;
        }

        // Generate duplikat enemy.
        Factory.FactoryMethod(spawnEnemy);
    }

    // Start the first wave.
    void StartWave() {

        // Start wave number 1.
        currentWave = 1;

        // Get current wave data.
        List<int> currentEnemyPool = enemyPool[currentWave - 1];
        currentWeight = waveWeight[currentWave - 1];
        
        // Get spawned enemy tag from currentEnemyPool until currentWeight is reached.
        while (spawnedWeight < currentWeight) {
            // Randomize spawned enemy without chance
            int enemyTag = currentEnemyPool[Random.Range(0, currentEnemyPool.Count)];
            if (spawnedWeight + enemyWeight[enemyTag] <= currentWeight) {
                spawnedEnemyTag.Add(enemyTag);
                spawnedWeight += enemyWeight[enemyTag];
            }
        }

        // Fill enemy amount.
        spawnedEnemyAmount = spawnedEnemyTag.Count;

        // Spawn the enemy two times faster than ZenMode.
        InvokeRepeating("SpawnWave", spawnTime, spawnTime / 2);
    }

    // Start the next wave.
    void NextWave() {

        // Reset the old wave data.
        spawnedEnemyTag.Clear();
        spawnedWeight = 0;
        spawnedEnemyAmount = 0;
        killedEnemyAmount = 0;
        
        // Increase current wave.
        currentWave++;

        // Get current wave data.
        List<int> currentEnemyPool = enemyPool[currentWave - 1];
        currentWeight = waveWeight[currentWave - 1];
        
        // Get spawned enemy tag from currentEnemyPool until currentWeight is reached.
        while (spawnedWeight < currentWeight) {
            // Randomize spawned enemy without chance
            int enemyTag = currentEnemyPool[Random.Range(0, currentEnemyPool.Count)];
            if (spawnedWeight + enemyWeight[enemyTag] <= currentWeight) {
                spawnedEnemyTag.Add(enemyTag);
                spawnedWeight += enemyWeight[enemyTag];
            }
        }

        // If the current wave is multiple of 3 then spawn boss.
        if (currentWave % 3 == 0) {
            // Spawn boss.
            spawnedEnemyTag.Insert(0, 5);
        }

        // Fill enemy amount.
        spawnedEnemyAmount = spawnedEnemyTag.Count;

        // Spawn the enemy two times faster than ZenMode.
        InvokeRepeating("SpawnWave", spawnTime, spawnTime / 2);
    }

    // Stop the wave mode.
    void StopWave() {
        // Cancel the invoke.
        CancelInvoke("SpawnWave");

        // Set gameWon
        gameWon = true;

        // Reset the wave data.
        spawnedEnemyTag.Clear();
        spawnedWeight = 0;
        spawnedEnemyAmount = 0;
        killedEnemyAmount = 0;
        // Reset current wave.
        //currentWave = 0;

        // Make player dead.
        playerHealth.TakeDamage(playerHealth.currentHealth);
    }

}