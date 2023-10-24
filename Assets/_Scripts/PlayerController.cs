using UnityEngine;

namespace Cplusiaki
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 4f;
        [Space(10)]
        [Range(0.01f, 15.0f)][SerializeField] private float jumpForce = 2f;

        private Rigidbody2D rb;
        private Animator animator;

        public LayerMask groundLayer;

        private float rayLength = 1.5f;

        private bool doubleJumpUsed = false;

        private bool isWalking = false;
        private bool isFacingRight = true;

        private int score = 0;

        private const string tagNameBonus = "Bonus";
        private const string isGroundedAnimationParameter = "isGrounded";
        private const string isWalkingAnimationParameter = "isWalking";

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            isWalking = false;

            //Right
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (!isFacingRight)
                    Flip();

                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
            }

            //Left
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (isFacingRight)
                    Flip();
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }

            animator.SetBool(isGroundedAnimationParameter, IsGrounded());
            animator.SetBool(isWalkingAnimationParameter, isWalking);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            int increaseValue = 10;
            if(other.CompareTag(tagNameBonus))
            {
                score += increaseValue;
                Debug.Log("Score: " + score);
                other.gameObject.SetActive(false);
            }
        }



        private void Jump()
        {
            if (IsGrounded())
            {
                doubleJumpUsed = false;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (!doubleJumpUsed)
            {
                doubleJumpUsed = true;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        private bool IsGrounded()
        {
            return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        }
 
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

        }
    }
}