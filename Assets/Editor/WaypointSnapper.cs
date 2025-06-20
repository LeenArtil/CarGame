
using UnityEngine;
using UnityEditor;

public class WaypointSnapper : EditorWindow
{
    [MenuItem("Tools/Snap Waypoints to Road %#s")] // Shortcut: Ctrl/Cmd + Shift + S
    public static void SnapWaypoints()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RaycastHit hit;
            Vector3 rayOrigin = obj.transform.position + Vector3.up * 10f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                Undo.RecordObject(obj.transform, "Snap Waypoint");
                obj.transform.position = hit.point + Vector3.up * 0.1f; // Slightly above road
                Debug.Log($"✅ Snapped {obj.name} to road at: {obj.transform.position}");
            }
            else
            {
                Debug.LogWarning($"❌ No ground found under {obj.name}");
            }
        }
    }
}
