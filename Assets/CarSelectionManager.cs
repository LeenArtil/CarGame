using UnityEngine;

public class CarSelectionManager : MonoBehaviour
{

    [Header("Race Mode")]
    public Transform raceSpawnPoint;
    public GameObject aiOpponentPrefab;
    private GameObject aiCar;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject carSelectionPanel;
    public GameObject mainMenuCanvas;

    [Header("Car Prefabs and Spawn")]
    public GameObject[] carPrefabs;
    public Transform spawnPoint;

    [Header("Camera")]
    public Camera mainCamera;

    private GameObject currentCar;
    private int selectedCarIndex = -1;
    private ScoreManager scoreManager;
    // 👇 At the top
    public GameObject scoreUI;

    private void Awake()
    {
        // Assign spawn point if not already set in Inspector
        if (spawnPoint == null)
        {
            GameObject fallback = GameObject.Find("CarSpawnPoint");
            if (fallback != null)
            {
                spawnPoint = fallback.transform;
                Debug.Log("🛠️ spawnPoint reassigned at runtime.");
            }
            else
            {
                Debug.LogError("❌ CarSpawnPoint not found in scene!");
            }
        }

        // Assign mainCamera if not set in Inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Debug.Log("🎥 mainCamera assigned using Camera.main: " + mainCamera.name);
            }
            else
            {
                GameObject fallbackCamera = GameObject.FindWithTag("MainCamera");
                if (fallbackCamera != null)
                {
                    mainCamera = fallbackCamera.GetComponent<Camera>();
                    Debug.Log("🎥 mainCamera assigned by tag fallback: " + fallbackCamera.name);
                }
                else
                {
                    Debug.LogError("❌ No camera found or assigned!");
                }
            }
        }

        // Ensure camera is tagged correctly
        if (mainCamera != null)
        {
            mainCamera.tag = "MainCamera";
            mainCamera.gameObject.SetActive(true);
            mainCamera.enabled = true;
            Debug.Log("✅ mainCamera is ready: " + mainCamera.name);
        }
    }


    private void Update()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning("❗ spawnPoint is NULL in Update() at time: " + Time.time);
        }
    }
    void Start()
    {
        Debug.Log("🎥 Camera.main: " + Camera.main);
        Camera[] cams = Camera.allCameras;
        foreach (Camera c in cams)
            Debug.Log("📸 Scene Camera: " + c.name + " | Tag: " + c.tag + " | Enabled: " + c.enabled);
    }


    public void OpenCarSelection()
    {
        mainMenuPanel.SetActive(false);
        carSelectionPanel.SetActive(true);
        scoreUI.SetActive(false);
    }

    private void AssignTagRecursively(Transform obj, string tag)
    {
        obj.tag = tag;
        foreach (Transform child in obj)
        {
            AssignTagRecursively(child, tag);
        }
    }

    public void SelectCar(int index)
    {
        selectedCarIndex = index;
        PlayerPrefs.SetInt("SelectedCarIndex", index);
        Debug.Log("🚗 Car selected: " + carPrefabs[index].name);
    }

    public void StartFreeMode()
    {
        ScoreManager.Instance?.ResetScore();
        scoreUI.SetActive(true);
        // Hide any leftover panels from previous session
        GameObject winPanel = GameObject.Find("WinPanel");
        GameObject losePanel = GameObject.Find("LosePanel");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        Time.timeScale = 1f;
        selectedCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", -1);

        var finish = FindObjectOfType<FinishLineTrigger>();
        if (finish != null) finish.ResetFinishLine();

        if (selectedCarIndex == -1)
        {
            Debug.LogWarning("❌ No car selected.");
            return;
        }

        // Destroy old car if exists
        if (currentCar != null)
        {
            Destroy(currentCar);
            currentCar = null;
        }

        // Destroy extra cameras EXCEPT mainCamera
        Camera[] allCams = Camera.allCameras;
        foreach (Camera cam in allCams)
        {
            if (cam != mainCamera)
            {
                Destroy(cam.gameObject);
                Debug.Log("🗑️ Destroyed extra camera: " + cam.name);
            }
        }

        // Spawn new car
        GameObject prefab = carPrefabs[selectedCarIndex];
        currentCar = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        AssignTagRecursively(currentCar.transform, "Player");

        // Reset mainCamera before attaching
        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(null); // Reset parent
            mainCamera.enabled = false;           // Reset state
            mainCamera.gameObject.SetActive(true); // Re-activate
        }

        // Re-parent camera to car
        if (mainCamera != null && currentCar != null)
        {
            mainCamera.transform.SetParent(currentCar.transform);
            mainCamera.transform.localPosition = new Vector3(0f, 2f, -6f);
            mainCamera.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);
            mainCamera.enabled = true;
            mainCamera.tag = "MainCamera";

            Debug.Log("📷 Camera parented to car and enabled.");
        }
        else
        {
            Debug.LogError("❌ mainCamera or currentCar is null during camera setup.");
        }

    

        // Hide UI
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        Debug.Log("✅ Free mode complete.");
    }

    public void StartRaceMode()
    {
        scoreUI.SetActive(false);
        // Hide Win/Lose panels if they exist
        GameObject winPanel = GameObject.Find("WinPanel");
        GameObject losePanel = GameObject.Find("LosePanel");
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);


        // Reset finish line trigger
        var finish = FindObjectOfType<FinishLineTrigger>();
        if (finish != null) finish.ResetFinishLine();


        Time.timeScale = 1f;

        selectedCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", -1);
        if (selectedCarIndex == -1)
        {
            Debug.LogWarning("❌ Please select a car first.");
            return;
        }

        // Assign RaceSpawnPoint if null
        if (raceSpawnPoint == null)
        {
            GameObject fallback = GameObject.Find("RaceSpawnPoint");
            if (fallback != null)
            {
                raceSpawnPoint = fallback.transform;
                Debug.Log("🛠️ raceSpawnPoint reassigned at runtime.");
            }
            else
            {
                Debug.LogError("❌ RaceSpawnPoint not found in scene!");
                return;
            }
        }

        // Detach camera before destroying car
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject fallbackCam = GameObject.FindWithTag("MainCamera");
                if (fallbackCam != null)
                {
                    mainCamera = fallbackCam.GetComponent<Camera>();
                    Debug.Log("🎥 Camera reassigned by tag fallback.");
                }
            }
        }

        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(null);
            if (mainCamera != null)
            {
                mainCamera.transform.SetParent(null, true);
                if (!mainCamera.gameObject.activeInHierarchy)
                    mainCamera.gameObject.SetActive(true);

                if (!mainCamera.enabled)
                    mainCamera.enabled = true;

                // Reset transform in case it's messed up
                mainCamera.transform.position = Vector3.zero;
                mainCamera.transform.rotation = Quaternion.identity;
            }

        }

        // Destroy previous car if exists
        if (currentCar != null)
        {
            Destroy(currentCar);
            currentCar = null;
        }

        // Spawn new player car
        GameObject prefab = carPrefabs[selectedCarIndex];
        currentCar = Instantiate(prefab, raceSpawnPoint.position, raceSpawnPoint.rotation);
        AssignTagRecursively(currentCar.transform, "Player");

        if (mainCamera != null)
        {
            // Reset camera transform
            mainCamera.transform.SetParent(currentCar.transform);
            mainCamera.transform.localPosition = new Vector3(0f, 2f, -6f);
            mainCamera.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);

            // Just in case: reset FOV and clipping planes
            mainCamera.fieldOfView = 60f;
            mainCamera.nearClipPlane = 0.3f;
            mainCamera.farClipPlane = 1000f;

            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);
            mainCamera.tag = "MainCamera";

            Debug.Log("📷 Camera properly attached and reset.");
        }


        // Hide menu
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        // Spawn AI opponent
        if (aiOpponentPrefab != null)
        {
            Vector3 aiSpawnPos = new Vector3(33.6f, 1.8f, 3.8f);
            Quaternion aiRotation = Quaternion.identity;
            aiCar = Instantiate(aiOpponentPrefab, aiSpawnPos, aiRotation);
            AssignTagRecursively(aiCar.transform, "AICar");

            var aiScript = aiCar.GetComponent<AICarWaypointFollower>();
            if (aiScript != null)
            {
                GameObject[] wps = GameObject.FindGameObjectsWithTag("Waypoint");
                System.Array.Sort(wps, (a, b) => a.name.CompareTo(b.name));
                aiScript.waypoints = System.Array.ConvertAll(wps, item => item.transform);
                aiScript.BeginRace();
            }
        }

        Debug.Log("🏁 Race Mode started with car: " + prefab.name);
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}