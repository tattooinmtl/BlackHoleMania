using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public GameObject startGameUI; // Drag and drop the StartGame UI GameObject here
    public string gameSceneName = "YourGameSceneName"; // Replace with the name of your game scene

    public void LoadGameScene()
    {
        if (startGameUI != null)
        {
            startGameUI.SetActive(false); // Hide the StartGame UI
        }
        else
        {
            Debug.LogError("StartGame UI not assigned in GameStarter!");
        }

        SceneManager.LoadScene(gameSceneName); // Load the game scene
    }
}