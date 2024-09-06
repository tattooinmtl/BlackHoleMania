using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text timerText;
    public Text creepCountText;
    public Text scoreText;
    public Text pointsText;
    public Text blackHoleSizeText;
    public Text lastEatenObjectText;
    public GameObject loseScreen;
    public Button restartButton;
    public GameObject loseCanvas;
    public GameObject winScreen; // Drag and drop your Win Screen UI GameObject here


    public GameController gameController;

    public static UIController Instance { get; private set; } 

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateTimerText();
        UpdateCreepCountText();
        UpdateScoreText();
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Round(gameController.currentTime).ToString();
        }
    }

    void UpdateCreepCountText()
    {
        if (creepCountText != null)
        {
            creepCountText.text = "Creeps: " + gameController.currentCreepCount.ToString();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + gameController.score.ToString();
        }
    }

    public void UpdateBlackHoleStats(int points, float blackHoleSize, string lastEatenObject)
    {
        if (pointsText != null)
            pointsText.text = "Points: " + points;
        if (blackHoleSizeText != null)
            blackHoleSizeText.text = "Black Hole Size: " + blackHoleSize.ToString("F2");
        if (lastEatenObjectText != null)
            lastEatenObjectText.text = "Last Eaten: " + lastEatenObject;
    }

    public void ShowLoseScreen()
{
    loseCanvas.SetActive(true); // Activate the loseCanvas
}
 public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}