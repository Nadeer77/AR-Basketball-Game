using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverUI;
    public TextMeshProUGUI highScoreText;
    private int highScore = 0;

    [Header("Lives UI")]
    public Image[] lifeIcons; // drag 5 images here

    private int score = 0;
    private int lives;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ResetGame();

        highScoreText.text = "High Score: " + highScore;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score;

        // ⭐ update high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);

            highScoreText.text = "High Score: " + highScore;
        }
    }

    public void LoseLife()
    {
        lives--;

        // 🔥 disable one icon
        if (lives >= 0 && lives < lifeIcons.Length)
        {
            lifeIcons[lives].enabled = false;
        }

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        ResetGame();

        BallFlickThrow throwScript = FindFirstObjectByType<BallFlickThrow>();
        if (throwScript != null)
        {
            throwScript.ResetThrowState();
        }

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in balls)
        {
            BallPool.Instance.ReturnBall(b); 
        }
    }

    void ResetGame()
    {
        score = 0;
        lives = lifeIcons.Length; // = 5

        scoreText.text = "Score: 0";

        // 🔥 enable all icons
        foreach (Image img in lifeIcons)
        {
            img.enabled = true;
        }

        gameOverUI.SetActive(false);
    }
}