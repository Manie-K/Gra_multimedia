using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Rendering;

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
        private int scoreIncreaseValueCoin = 10;
        private int scoreIncreaseValueEnemy = 50;

        private int keysFound = 0;
        private int keysNumber = 3;

        private int lives = 3;

        private Vector2 startPosition;

        private const string tagNameBonus = "Bonus";
        private const string tagNameEnemy = "Enemy";
        private const string tagNameLevelFinish = "Finish";
        private const string tagNameKey = "Key";
        private const string tagNameHeart = "Heart";
        private const string tagNameFallLevel = "FallLevel";

        private const string isGroundedAnimationParameter = "isGrounded";
        private const string isWalkingAnimationParameter = "isWalking";

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            startPosition = transform.position;
        }

        private void Update()
        {
            HandleInput();
            HandleJump();
            HandleAnimation();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(tagNameBonus))
            {
                score += scoreIncreaseValueCoin;
                other.gameObject.SetActive(false);
                
                Debug.Log("Score: " + score);
                return;
            }
            if (other.CompareTag(tagNameLevelFinish))
            {
                if(keysFound == keysNumber)
                {
                    Debug.Log("Player won the game!");
                }
                else
                {
                    Debug.Log($"Player has to collect all keys! Current progress: {keysFound}/{keysNumber}");
                }
                return;
            }
            if (other.CompareTag(tagNameEnemy))
            {
                if (transform.position.y > other.gameObject.transform.position.y)
                {
                    score += scoreIncreaseValueEnemy;
                    Debug.Log("Killed an enemy!");
                }
                else
                {
                    Death();
                }
                return;
            }
            if (other.CompareTag(tagNameKey))
            {
                keysFound++;
                Debug.Log($"Keys found: {keysFound}/{keysNumber}");
                other.gameObject.SetActive(false);
                return;
            }
            if (other.CompareTag(tagNameHeart))
            {
                lives++;
                Debug.Log($"Life added. Lives left: {lives}");
                other.gameObject.SetActive(false);
                return;
            }
            if (other.CompareTag(tagNameFallLevel))
            {
                Death();
                return;
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
                Vector3 tempVelocity = rb.velocity;
                tempVelocity.y = 0f;
                rb.velocity = tempVelocity;
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

        private void Death()
        {
            lives--;
            if (lives > 0)
            {
                transform.position = startPosition;
                rb.velocity = Vector3.zero;
                Debug.Log($"You died! Lives left: {lives}");
            }
            else
            {
                Debug.Log("GAME HAS ENDED, PLAYER LOST!!!");
            }
        }
    }
}