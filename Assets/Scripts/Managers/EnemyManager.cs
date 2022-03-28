using UnityEngine;

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


    void Start ()
    {
        // Mengeksekusi fungsi spawn() setiap spawnTime.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        // Jika player telah mati, maka tidak melakukan spawn.
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        // Mendapatkan nilai random.
        int spawnEnemy = Random.Range(0, 3);

        // Generate duplikat enemy.
        Factory.FactoryMethod(spawnEnemy);
    }
}
