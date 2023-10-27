using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float distanceZone;
    [SerializeField] private float speed = 1.5f;

    private Vector2 targetA;
    private Vector2 targetB;
    private Vector2 target;

    private void Awake()
    {
        target = targetA = pointA.position;
        targetB = pointB.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime*speed);
        float distanceToTarget = Vector3.Distance(transform.position, target);

        if (distanceToTarget <= distanceZone )
        {
            target = target == targetA ? targetB : targetA;
        }
    }
}
