using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 👈 Singleton

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUI;

    private bool isGameOver = false;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        score = 0;
        UpdateScore();

        gameOverUI.SetActive(false);
        isGameOver = false;
    }

    public void AddScore()
    {
        if (isGameOver) return;

        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        score = 0;
        UpdateScore();

        gameOverUI.SetActive(false);
        isGameOver = false;

        // 🔥 Reset throw system
        BallFlickThrow throwScript = FindFirstObjectByType<BallFlickThrow>();
        if (throwScript != null)
        {
            throwScript.ResetThrowState();
        }

        // 🔥 Remove old balls
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }
}