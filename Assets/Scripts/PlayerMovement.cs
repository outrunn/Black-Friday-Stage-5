using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;            // Units per second
    [SerializeField] private bool normalizeDiagonal = true;   // Keep diagonal speed consistent

    private Rigidbody2D rb;
    private Vector2 input;           // raw input each frame

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;        // top-down 2D
        rb.freezeRotation = false;   // we control rotation in code
    }

    private void Update()
    {
        // Read input (WASD/Arrow Keys by default Unity axes)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y);

        if (input != Vector2.zero)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    private void FixedUpdate()
    {
        // --- Movement ---
        Vector2 velocity = input;
        if (normalizeDiagonal && velocity.sqrMagnitude > 1f)
            velocity = velocity.normalized;

        rb.linearVelocity = velocity * moveSpeed;
    }
}
