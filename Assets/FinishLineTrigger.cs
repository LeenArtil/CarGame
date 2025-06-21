// ✅ FinishLineTrigger.cs – Updated to support reset
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject mainMenuCanvas;
    public GameObject menuPanel;
    public GameObject carSelectionPanel;

    [Header("Camera Settings")]
    public Camera mainCamera;
    public Vector3 panelOffset = new Vector3(0, 2, 3);

    private bool raceEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (raceEnded) return;
        raceEnded = true;

        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(false);
        if (carSelectionPanel != null) carSelectionPanel.SetActive(false);

        if (other.CompareTag("Player"))
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
        else if (other.CompareTag("AICar"))
        {
            if (losePanel != null) losePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ResetFinishLine()
    {
        raceEnded = false;
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (carSelectionPanel != null) carSelectionPanel.SetActive(false);
    }
}
