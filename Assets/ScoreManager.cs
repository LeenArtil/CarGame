using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton
    public static ScoreManager Instance { get; private set; }

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Score State")]
    public int score = 0;
    public bool isCounting = false;

    private int bestScore = 0;

    private void Awake()
    {
        // Singleton logic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional if persisting between scenes
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
    }

    private void Update()
    {
        if (isCounting)
        {
            score += 1;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void ResetScore()
    {
        score = 0;
        isCounting = true;
        UpdateScoreUI();
    }

    public void StopAndCompare()
    {
        isCounting = false;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        Debug.Log($"🛑 Score Stopped at: {score} | Best: {bestScore}");
    }

    public int GetBestScore()
    {
        return bestScore;
    }
}
