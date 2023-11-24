using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1f;

    private float moveThreeshold = 0.1f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if(Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < moveThreeshold)
        {
            currentWaypointIndex++;
            currentWaypointIndex %= waypoints.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
