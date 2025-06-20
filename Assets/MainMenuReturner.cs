using UnityEngine;
using System.Linq; // 🔧 Required for FirstOrDefault()

public class MainMenuReturner : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject menuPanel;
    public GameObject carSelectionPanel;

    public static MainMenuReturner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Camera[] cams = Camera.allCameras;
        Debug.Log("🔍 Total active cameras in scene: " + cams.Length);

        foreach (Camera c in cams)
            Debug.Log("📸 Camera: " + c.name + ", enabled: " + c.enabled + ", tag: " + c.tag);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 0f;

        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (carSelectionPanel != null) carSelectionPanel.SetActive(false);

        var cam = Camera.main;
        if (cam == null)
        {
            cam = Camera.allCameras.FirstOrDefault(c => c.enabled); // ✅ now this works
            Debug.LogWarning("⚠️ Camera.main was null. Found fallback camera: " + cam?.name);
        }

        if (cam != null)
        {
            cam.enabled = true;
            cam.gameObject.SetActive(true);
            cam.tag = "MainCamera";

            if (cam.transform.parent != null)
                cam.transform.SetParent(null, true);

            cam.transform.position = new Vector3(0f, 5f, -15f);
            cam.transform.rotation = Quaternion.Euler(10f, 0f, 0f);
        }
        else
        {
            Debug.LogError("❌ No active camera found! Scene has no working camera.");
        }

        DestroyExistingCars();
        Debug.Log("🏁 Returned to Main Menu after crash.");
    }

    private void DestroyExistingCars()
    {
        var playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar != null) Destroy(playerCar);

        var aiCar = GameObject.FindGameObjectWithTag("AICar");
        if (aiCar != null) Destroy(aiCar);
    }
}
