using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton
    public static ScoreManager Instance { get; private set; }

    [Header("Score UI")]
    public TextMeshProUGUI currentScoreText;  // Shown in Free Mode
    public TextMeshProUGUI bestScoreText;     // Shown in Main Menu

    [Header("Score State")]
    public int score = 0;
    public bool isCounting = false;

    private int bestScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
    }

    private void Update()
    {
        if (!isCounting) return;

        // ✅ Increase score only when W or UpArrow is pressed
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            score += 1;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        // Update Free Mode Score
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + score;

        // Update Main Menu Best Score
        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore;
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

        UpdateScoreUI();
        Debug.Log($"🛑 Score Stopped at: {score} | Best: {bestScore}");
    }

    public int GetBestScore()
    {
        return bestScore;
    }

    public void SetBestScoreTextReference(TextMeshProUGUI bestText)
    {
        bestScoreText = bestText;
        UpdateScoreUI();
    }

    public void SetCurrentScoreTextReference(TextMeshProUGUI currentText)
    {
        currentScoreText = currentText;
        UpdateScoreUI();
    }
}
