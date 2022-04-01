using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;


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
        // TODO: Add skeleton(3), bomber(4), and boss(5).
        0, // Zombear
        1, // Zombunny
        2, // Zomhellephant
    };
    
    // Weight of each enemy.
    public static readonly List<int> enemyWeight = new List<int>() {
        1,
        1,
        2,
    };

    // Max wave.
    public const int maxWave = 10;

    // TODO : Add new enemies to the list.
    // Declare pool of enemy.
    public static readonly List<List<int>> enemyPool = new List<List<int>>() {
        new List<int>() { 0 }, // Wave 1
        new List<int>() { 0, 1 }, // Wave 2
        new List<int>() { 0, 1 }, // Wave 3
        new List<int>() { 0, }, // Wave 4
        new List<int>() { 0, 1, }, // Wave 5
        new List<int>() { 0, 1 }, // Wave 6
        new List<int>() { 0 }, // Wave 7
        new List<int>() { 0, 1 }, // Wave 8
        new List<int>() { 0, 1 }, // Wave 9
        new List<int>() { 0, 1, 2 }, // Wave 10
    };
    
    // Wave weight.
    // TODO: make the wave more harder.
    public static readonly List<int> waveWeight = new List<int>() {
        // 5, 
        // 10,
        // 16,
        // 23,
        // 31,
        // 40,
        // 50,
        // 61,
        // 73,
        1, 
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
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
            InvokeRepeating ("SpawnZen", spawnTime, spawnTime);
        }
        
        if (GameModeManager.gameMode == GameMode.Wave)
        {
            // If the game is wave mode then start the wave.
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

            // If the killed enemy amount is equal to the spawned enemy amount adn wave has 
            // been started then start the next wave.
            if (killedEnemyAmount == spawnedEnemyAmount)
            {
                // If current finished wave is divisible by 3 then let player to upgrade weapon.
                if (currentWave % 3 == 0)
                {
                    WeaponUpgradeManager.maxUpgrade++;
                }
                NextWave();
            }

            // If the current wave is max wave, then stop the wave.
            if (currentWave > maxWave)
            {
                StopWave();
            }
        }

        // If the game is zen mode check some event.
        if (GameModeManager.gameMode == GameMode.Zen)
        {
            // Add 1 upgrade point every 30 second and make the spawning faster.
            if (Math.Floor(Math.Floor(ScoreManager.survival_time) / 30) > WeaponUpgradeManager.maxUpgrade )
            {
                Debug.Log("Add 1 upgrade point");
                WeaponUpgradeManager.maxUpgrade++;
                // Divide spawn time by 2
                spawnTime /= (float)(Math.Floor(Math.Floor(ScoreManager.survival_time) / 30));
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

        // TODO: Add new enemy and boss in randomization
        // Randomize enemy with chance of each enemy.
        // Zombunny (tag-0) = 40%
        // Zombear (tag-1) = 40%
        // Zomhellephant (tag-2) = 20%
        int chance = Random.Range (0, 100);
        if (chance < 40)
        {
            // Spawn Zombunny.
            spawnEnemy = 0;
        }
        else if (chance < 80)
        {
            // Spawn Zombear.
            spawnEnemy = 1;
        }
        else
        {
            // Spawn Zomhellephant.
            spawnEnemy = 2;
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
            // TODO: Change to spawn real boss
            // Spawn boss.
            spawnedEnemyTag.Insert(0, 2);
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
        // Reset the wave data.
        spawnedEnemyTag.Clear();
        spawnedWeight = 0;
        spawnedEnemyAmount = 0;
        killedEnemyAmount = 0;
        // Reset current wave.
        currentWave = 0;

        // TODO: Instead of making player dead, end the wave mode game.
        // Make player dead.
        playerHealth.TakeDamage(playerHealth.currentHealth);
    }

}