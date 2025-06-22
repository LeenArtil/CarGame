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

        // Reactivate UI
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(true);
        if (carSelectionPanel != null) carSelectionPanel.SetActive(false);

        // Get the main camera
        var cam = Camera.main;
        if (cam == null)
        {
            cam = Camera.allCameras.FirstOrDefault(c => c.enabled);
            Debug.LogWarning("⚠️ No Camera.main found. Fallback: " + cam?.name);
        }


  


        // Detach from player and reset camera
        if (cam != null)
        {
            cam.transform.SetParent(null);
            cam.enabled = true;
            cam.gameObject.SetActive(true);
            cam.tag = "MainCamera";

            cam.transform.position = new Vector3(0f, 5f, -15f);
            cam.transform.rotation = Quaternion.Euler(10f, 0f, 0f);
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 1000f;
            cam.fieldOfView = 60f;

            // Reassign to CarSelectionManager
            var csm = FindObjectOfType<CarSelectionManager>();
            if (csm != null)
            {
                csm.mainCamera = cam;
            }
        }

        // 🚫 DO NOT destroy the main camera! Only cars.
        DestroyExistingCars();

        Debug.Log("🏁 Returned to Main Menu.");
    }

    private void DestroyExistingCars()
    {
        var playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar != null) Destroy(playerCar);

        var aiCar = GameObject.FindGameObjectWithTag("AICar");
        if (aiCar != null) Destroy(aiCar);
    }
}
