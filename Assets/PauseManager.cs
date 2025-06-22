// PauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Assign this in the inspector
    private bool isPaused = false;
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject carSelectionPanel;

    public GameObject scoreUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Resume time
        MainMenuReturner.Instance.ReturnToMainMenu(); // SAFE and tested
        pausePanel.SetActive(false);
    }

    private void TogglePause()
    {
        isPaused = !isPaused; // Flip the pause state

        if (isPaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true); // Show pause panel
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false); // Hide pause panel
        }
    }

}
