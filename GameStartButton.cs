using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public StartGameScript startGameScript; 

    public void OnStartButtonClicked()
    {
        if (startGameScript != null) 
        {
            startGameScript.startGameUI.SetActive(true); 
            // Add any other game start logic here 
        }
        else
        {
            Debug.LogError("StartGameScript is not assigned to the StartGameButton!");
        }
    }
}