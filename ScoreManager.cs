using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        score += points;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreDisplay();
    }
}