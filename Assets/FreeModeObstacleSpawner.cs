// 🚧 FreeModeWaypointObstacleSpawner.cs – Spawns 1 obstacle per waypoint if surface is '2RoadMob'
using UnityEngine;

public class FreeModeWaypointObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public Transform[] waypoints; // 👈 Drag your waypoint objects here manually
    public string targetMaterialName = "2RoadMob";
    public LayerMask roadLayer;

    void Start()
    {
        int spawned = 0;

        foreach (var wp in waypoints)
        {
            if (wp == null) continue;

            RaycastHit hit;
            Vector3 rayOrigin = wp.position + Vector3.up * 10f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f, roadLayer))
            {
                var renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null && renderer.sharedMaterial != null &&
                    renderer.sharedMaterial.name.Contains(targetMaterialName))
                {
                    // Spawn an obstacle directly on the waypoint
                    GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                    Vector3 spawnPos = hit.point + Vector3.up * 0.1f;
                    Quaternion rot = Quaternion.LookRotation(wp.forward, Vector3.up);
                    Instantiate(prefab, spawnPos, rot);
                    spawned++;
                }
            }
        }

        Debug.Log($"✅ Spawned {spawned} obstacles on valid waypoints.");
    }
}
