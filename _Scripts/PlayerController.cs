using UnityEngine;

namespace Cplusiaki
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 4f;
        [Range(0.01f, 15.0f)][SerializeField] private float jumpForce = 2f;
        [Range(0.1f, 3.0f)][SerializeField] private float doubleJumpMultiplier = 1.5f;
        [Range(0.01f, 10.0f)][SerializeField] private float gravityNormal = 1.25f;
        [Range(0.01f, 10.0f)][SerializeField] private float gravityBoosted = 2.25f;
        [Space(10)]
        [SerializeField] private Transform downRayLeft, downRayRight;

        public LayerMask groundLayer;

        private Rigidbody2D rb;
        private Animator animator;

        private float rayLength = 1.5f;

        private bool doubleJumpUsed = false;

        private bool isWalking = false;
        private bool isFacingRight = true;
        private bool isJumping = false;

        private int score = 0;
        private int scoreIncreaseValue = 10;

        private const string tagNameBonus = "Bonus";
        private const string tagNameFinish = "Finish";
        private const string isGroundedAnimationParameter = "isGrounded";
        private const string isWalkingAnimationParameter = "isWalking";

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            HandleInput();
            HandleJump();
            HandleAnimation();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(tagNameBonus))
            {
                score += scoreIncreaseValue;
                other.gameObject.SetActive(false);
                
                Debug.Log("Score: " + score);
            }
            if (other.CompareTag(tagNameBonus))
            {
                Debug.Log("You have finished this level!");
            }
        }

        private void Jump()
        {
            if (IsGrounded())
            {
                doubleJumpUsed = false;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
            }
            else if (!doubleJumpUsed)
            {
                doubleJumpUsed = true;
                rb.AddForce(Vector2.up * jumpForce * doubleJumpMultiplier, ForceMode2D.Impulse);
                isJumping = true;
            }
        }
        private bool IsGrounded()
        {
            bool touchingGround = Physics2D.Raycast(downRayLeft.position, Vector2.down, rayLength, groundLayer.value);

            if(!touchingGround)
            {
                touchingGround = Physics2D.Raycast(downRayRight.position, Vector2.down, rayLength, groundLayer.value);
            }

            return touchingGround;
        }
 
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void HandleInput()
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
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        private void HandleJump()
        {
            if (isJumping)
            {
                if (IsGrounded()) //deactivate isJumping
                {
                    isJumping = false;
                }
                else if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravityBoosted;
                }
                else if (rb.velocity.y >= 0)
                {
                    rb.gravityScale = gravityNormal;
                }
            }
            else
            {
                rb.gravityScale = gravityNormal;
            }
        }

        private void HandleAnimation()
        {
            animator.SetBool(isGroundedAnimationParameter, IsGrounded());
            animator.SetBool(isWalkingAnimationParameter, isWalking);
        }
    }
}