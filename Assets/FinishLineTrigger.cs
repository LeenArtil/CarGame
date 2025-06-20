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
    public Vector3 panelOffset = new Vector3(0, 2, 3); // Position in front of camera

    private bool raceEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (raceEnded) return;
        raceEnded = true;

        Debug.Log("🏁 Trigger entered by: " + other.name);

        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(false);
        if (carSelectionPanel != null) carSelectionPanel.SetActive(false);

        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 PLAYER WINS!");
            if (winPanel != null)
            {
                PositionUIPanel(winPanel);
                winPanel.SetActive(true);
            }
        }
        else if (other.CompareTag("AICar"))
        {
            Debug.Log("🤖 AI WINS!");
            if (losePanel != null)
            {
                PositionUIPanel(losePanel);
                losePanel.SetActive(true);
            }
        }

        Time.timeScale = 0f;
    }

    private void PositionUIPanel(GameObject panel)
    {
        if (mainCamera == null) return;

        // Place UI panel in front of the camera
        Vector3 worldPosition = mainCamera.transform.position + mainCamera.transform.forward * panelOffset.z + Vector3.up * panelOffset.y;
        panel.transform.position = worldPosition;

        // Make panel face the camera
        panel.transform.LookAt(mainCamera.transform);
        panel.transform.rotation = Quaternion.LookRotation(panel.transform.position - mainCamera.transform.position);
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
