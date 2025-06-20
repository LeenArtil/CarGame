// 🚗 CarSelectionManager.cs – Handles car picking logic + mode selection
using UnityEngine;
using System.Collections;
using System.Linq;
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

    private void Awake()
    {
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
                Debug.LogError("❌ CarSpawnPoint not found!");
            }
        }

        if (mainCamera != null)
        {
            mainCamera.tag = "MainCamera";
        }
    }

    private void Update()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning("❗ spawnPoint is NULL in Update() at time: " + Time.time);
        }
    }
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);
            Debug.Log("✅ Forced main camera on start.");
        }
    }


    public void OpenCarSelection()
    {
        mainMenuPanel.SetActive(false);
        carSelectionPanel.SetActive(true);
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

    public IEnumerator StartFreeMode()
    {
        Time.timeScale = 1f;

        selectedCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", -1);
        Debug.Log("🟢 StartFreeMode called. selectedCarIndex = " + selectedCarIndex);

        if (selectedCarIndex == -1)
        {
            Debug.LogWarning("❌ No car selected.");
            yield break;
        }

        if (currentCar != null)
        {
            if (mainCamera != null)
                mainCamera.transform.SetParent(null);

            Destroy(currentCar);
            currentCar = null;
            Debug.Log("🗑️ Previous car destroyed.");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("🚨 spawnPoint is still null!");
            yield break;
        }

        GameObject prefab = carPrefabs[selectedCarIndex];
        if (prefab == null)
        {
            Debug.LogError("🚨 Prefab at index " + selectedCarIndex + " is null!");
            yield break;
        }

        currentCar = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        AssignTagRecursively(currentCar.transform, "Player");
        Debug.Log("🚗 Car Instantiated at: " + currentCar.transform.position);

        yield return null; // ⏳ Wait one frame for proper camera setup

        if (mainCamera != null && currentCar != null)
        {
            mainCamera.tag = "MainCamera";
            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);

            mainCamera.transform.SetParent(currentCar.transform);
            mainCamera.transform.localPosition = new Vector3(0f, 2f, -6f);
            mainCamera.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);

            StartCoroutine(DisableExtraCamerasNextFrame());
        }
        else
        {
            Debug.LogError("❌ mainCamera or currentCar is null during camera assignment.");
        }

        scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
            Debug.Log("🎯 Score reset.");
        }

        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        Debug.Log("✅ Free mode complete.");
    }
    public void StartFreeModeButton()
    {
        if (!IsInvoking(nameof(DelayedStartFreeMode)))
        {
            Invoke(nameof(DelayedStartFreeMode), 0.01f); // tiny delay
        }
    }

    private void DelayedStartFreeMode()
    {
        StartCoroutine(StartFreeMode());
    }


    //public void StartRaceMode()
    //{
    //    if (selectedCarIndex == -1)
    //    {
    //        Debug.LogWarning("❌ Please select a car first.");
    //        return;
    //    }

    //    if (mainCamera != null)
    //    {
    //        mainCamera.transform.SetParent(null);
    //    }

    //    if (currentCar != null)
    //    {
    //        Destroy(currentCar);
    //        currentCar = null;
    //    }

    //    currentCar = Instantiate(carPrefabs[selectedCarIndex], raceSpawnPoint.position, raceSpawnPoint.rotation);
    //    AssignTagRecursively(currentCar.transform, "Player");

    //    if (mainCamera != null)
    //    {
    //        mainCamera.transform.position = raceSpawnPoint.position + new Vector3(0f, 10f, -15f);
    //        mainCamera.transform.LookAt(raceSpawnPoint.position + new Vector3(0f, 0f, 10f));
    //        mainCamera.enabled = true;
    //        mainCamera.gameObject.SetActive(true);
    //        mainCamera.tag = "MainCamera";
    //    }

    //    if (mainMenuCanvas != null)
    //        mainMenuCanvas.SetActive(false);

    //    if (aiOpponentPrefab != null)
    //    {
    //        Vector3 aiSpawnPos = new Vector3(33.6f, 1.8f, 3.8f);
    //        Quaternion aiRotation = Quaternion.Euler(0f, 0f, 0f);

    //        aiCar = Instantiate(aiOpponentPrefab, aiSpawnPos, aiRotation);
    //        AssignTagRecursively(aiCar.transform, "AICar");

    //        var aiScript = aiCar.GetComponent<AICarWaypointFollower>();
    //        if (aiScript != null)
    //        {
    //            GameObject[] wps = GameObject.FindGameObjectsWithTag("Waypoint");
    //            System.Array.Sort(wps, (a, b) => a.name.CompareTo(b.name));
    //            aiScript.waypoints = System.Array.ConvertAll(wps, item => item.transform);
    //            aiScript.BeginRace();
    //        }
    //    }

    //    Debug.Log("🏁 Race mode started with car: " + carPrefabs[selectedCarIndex].name);
    //}
    public void StartRaceMode()
    {
        if (selectedCarIndex == -1)
        {
            Debug.LogWarning("Please select a car first.");
            return;
        }
        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(null); // Detach camera
        }

        if (currentCar != null)
            Destroy(currentCar);

        // 🚗 Spawn Player Car at race spawn point
        currentCar = Instantiate(carPrefabs[selectedCarIndex], raceSpawnPoint.position, raceSpawnPoint.rotation);
        AssignTagRecursively(currentCar.transform, "Player");
        // 🎥 Move camera above and behind race start
        if (mainCamera != null)
        {
            mainCamera.transform.position = raceSpawnPoint.position + new Vector3(0f, 10f, -15f);
            mainCamera.transform.LookAt(raceSpawnPoint.position + new Vector3(0f, 0f, 10f));
        }

        // 🎮 Hide menu canvas
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        // 🤖 Spawn AI Car at manually aligned position (on the road)
        if (aiOpponentPrefab != null)
        {
            Vector3 aiSpawnPos = new Vector3(33.6f, 1.8f, 3.8f); // 👈 slightly raised to prevent wheel clipping
                                                                 // ✅ Your tested perfect position
            Quaternion aiRotation = Quaternion.Euler(0f, 0f, 0f); // Facing forward
            aiCar = Instantiate(aiOpponentPrefab, new Vector3(33.6f, 1.8f, 3.8f), Quaternion.identity);
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

        Debug.Log("Race mode started with car: " + carPrefabs[selectedCarIndex].name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


   private IEnumerator DisableExtraCamerasNextFrame()
{
    yield return null; // Wait 1 frame for camera registration

    if (mainCamera == null)
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("❌ mainCamera is still null after waiting!");
            yield break;
        }
    }

    foreach (Camera cam in Camera.allCameras)
    {
        // ✅ Only disable cameras not on the same GameObject as your mainCamera
        if (cam != mainCamera)
        {
            cam.enabled = false;
            Debug.Log("❌ Disabled extra camera: " + cam.name);
        }
        else
        {
            cam.enabled = true;
            Debug.Log("✅ Kept main camera: " + cam.name);
        }
    }

    Debug.Log("✅ All extra cameras disabled properly.");
}


}