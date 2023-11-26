using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 4f;
    
    [Header("Starting settings")]
    [SerializeField] private float startPositionX;

    public float moveRange = 1f;

    private bool isMovingRight = false;
    private bool isFacingRight = false;
    
    private Animator animator;
    private float deadAnimationDelayTime = 1f;

    private const string tagNamePlayer = "Player";

    private const string isDeadAnimationParameter = "isDead";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startPositionX = transform.position.x;
    }

    private void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x + startPositionX > moveRange)
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
            if(startPositionX + transform.position.x < -moveRange)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(tagNamePlayer))
        {
            if(other.gameObject.transform.position.y > transform.position.y)
            {
                animator.SetBool(isDeadAnimationParameter, true);
                StartCoroutine(KillOnAnimationEnd());
                
                
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(deadAnimationDelayTime);
        gameObject.SetActive(false);
    }

    private void MoveRight()
    {
        if (!isFacingRight)
            Flip();

        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void MoveLeft()
    {
        if (isFacingRight)
            Flip();
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
