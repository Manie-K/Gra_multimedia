using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 4f;

    private float startPositionX;

    public float moveRange = 3f;

    private bool isMovingRight = false;


    private void Awake()
    {
        startPositionX = transform.position.x;
    }

    private void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x > startPositionX + moveRange)
            {
                isMovingRight = false;
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
        else
        {
            if (transform.position.x < startPositionX - moveRange)
            {
                isMovingRight = true;
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }
    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
}
