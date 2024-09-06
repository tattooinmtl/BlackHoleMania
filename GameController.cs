using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{
    public SpawnPoint[] spawnPoints;
    public int creepsPerWave = 10;
    public float timeBetweenWaves = 5f;
    public float timeBetweenSpawns = 0.2f;
    public float gameTime = 60f; // Initial timer value (for win condition)
    public int maxCreeps = 100;

    public int score = 0;
    public int currentCreepCount = 0;
    public float currentTime;
    private bool gameOver = false;

    public static GameController Instance;

    public TMP_Text roundText;
    public TMP_Text timerText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentTime = gameTime;
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (!gameOver)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                WinGame();
            }

            // Update UI elements
            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.Round(currentTime).ToString();
            }

            currentCreepCount = FindObjectsOfType<EatableObject>().Length;
            if (currentCreepCount >= maxCreeps)
            {
                LoseGame();
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        int currentRound = 1;

        while (true) // Spawn waves endlessly
        {
            if (currentCreepCount < maxCreeps)
            {
                for (int i = 0; i < creepsPerWave; i++)
                {
                    if (spawnPoints.Length > 0 && currentCreepCount < maxCreeps)
                    {
                        int spawnIndex = Random.Range(0, spawnPoints.Length);

                        // Check if SpawnPoint still exists
                        if (spawnPoints[spawnIndex] != null)
                        {
                            spawnPoints[spawnIndex].SpawnCreep();
                            yield return new WaitForSeconds(timeBetweenSpawns);
                            currentCreepCount++;
                        }
                        else
                        {
                            Debug.LogWarning("SpawnPoint at index " + spawnIndex + " has been destroyed!");
                        }
                    }
                }
            }

            if (roundText != null)
            {
                roundText.text = "Round: " + currentRound;
            }

            currentRound++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    public void ObjectEaten(int scoreValue)
    {
        score += scoreValue;
        currentTime += 1f; // Add 1 second to the timer HERE!
    }

    void WinGame()
    {
        gameOver = true;
        Debug.Log("You Win!");

        // Stop spawning waves 
        StopCoroutine(SpawnWaves());

        // Show win screen using UIController
        UIController uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            uiController.ShowWinScreen(); // Assuming you have a ShowWinScreen method
        }
        else
        {
            Debug.LogError("UIController not found in the scene!");
        }
    }

    void LoseGame()
    {
        gameOver = true;
        Debug.Log("You Lose!");

        // Stop spawning waves
        StopCoroutine(SpawnWaves());

        // Show lose screen using UIController
        UIController uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            uiController.ShowLoseScreen(); 
        }
        else
        {
            Debug.LogError("UIController not found in the scene!");
        }
    }
}