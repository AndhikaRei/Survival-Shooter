using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public GameOverManager gameOverManager;

    private void OnTriggerEnter(Collider other)
    {
        // Write in log.
        if (other.tag == "Enemy" && !other.isTrigger)
        {
            float enemyDistance = Vector3.Distance(transform.position,other.transform.position);
            gameOverManager.ShowWarning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Write in log.
        if (other.tag == "Enemy" && !other.isTrigger)
        {
            gameOverManager.RemoveWarning();
        }
    }
}
