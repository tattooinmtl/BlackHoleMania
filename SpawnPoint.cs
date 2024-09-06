using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] creepPrefabs; // Array to hold different creep prefabs
    private int currentCreepIndex = 0; 

    public void IncreaseDifficulty()
    {
        currentCreepIndex++;
        if (currentCreepIndex >= creepPrefabs.Length)
        {
            currentCreepIndex = creepPrefabs.Length - 1; // Loop back to the strongest creep
        }

        // Optional: Adjust other spawn parameters in GameController
         GameController.Instance.timeBetweenSpawns *= 0.9f;
         GameController.Instance.creepsPerWave += 2; 
    }

    public void SpawnCreep()
    {
        if (creepPrefabs.Length > 0)
        {
            Instantiate(creepPrefabs[currentCreepIndex], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No creep prefabs assigned to SpawnPoint: " + gameObject.name);
        }
    }
}