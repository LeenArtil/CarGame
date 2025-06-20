using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AICarWaypointFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 10f;
    public float turnSpeed = 5f;

    private int currentIndex = 0;
    private Rigidbody rb;
    private bool raceStarted = false;

    public void BeginRace()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody not found!");
            return;
        }

        raceStarted = true;
        Debug.Log("✅ AI BeginRace triggered.");
    }

    void FixedUpdate()
    {
        if (!raceStarted || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // Move forward
        rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);

        // Turn toward the next point
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime));

        // Go to next waypoint if close enough
        if (Vector3.Distance(transform.position, target.position) < 3f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                raceStarted = false;
                Debug.Log("🏁 AI finished the race.");
            }
        }
    }
}
