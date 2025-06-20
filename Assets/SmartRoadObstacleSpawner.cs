using UnityEngine;

public class SmartRoadObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Prefabs to spawn
    public int obstacleCount = 100;      // Total number of obstacles
    public float zStart = 0f;            // Start of the road
    public float zEnd = 2700f;           // End of the road
    public float spacing = 25f;          // Distance between spawn attempts
    public float xMin = 20f;             // Wide X range that covers road + grass
    public float xMax = 40f;
    public float raycastHeight = 50f;    // Height to raycast down from

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        int spawned = 0;

        for (float z = zStart; z <= zEnd && spawned < obstacleCount; z += spacing)
        {
            float x = Random.Range(xMin, xMax);
            Vector3 guessPos = new Vector3(x, raycastHeight, z);
            Vector3 rayDirection = Vector3.down;

            if (Physics.Raycast(guessPos, rayDirection, out RaycastHit hit, 100f))
            {
                Debug.DrawRay(guessPos, Vector3.down * 100f, Color.red, 2f);
                Debug.Log("Hit: " + hit.collider.gameObject.name + " at Z = " + z);

                Renderer renderer = hit.collider.GetComponent<Renderer>();

                if (renderer != null && renderer.sharedMaterial != null &&
                    renderer.sharedMaterial.name.Contains("2RoadMob")) // ✅ Only spawn on road
                {

                    Debug.Log("✅ Spawning on road at: " + hit.point);

                    Vector3 spawnPos = hit.point + Vector3.up * 0.05f;

                    GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                    Instantiate(prefab, spawnPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0f));

                    spawned++;
                }
            }
        }

        Debug.Log($"✅ Spawned {spawned} obstacles on road.");
    }

}
