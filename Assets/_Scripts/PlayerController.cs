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
        public LayerMask groundLayer;

        private float rayLength = 1.5f;

        private bool doubleJumpUsed = false;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }
        private void Jump()
        {
            if (IsGrounded())
            {
                doubleJumpUsed = false;
                Debug.Log("JUMPING");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (!doubleJumpUsed)
            {
                doubleJumpUsed = true;
                Debug.Log("Double Jump");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        private bool IsGrounded()
        {
            return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        }
    }
}