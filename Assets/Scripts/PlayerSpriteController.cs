using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteController : MonoBehaviour
{
    [Header("Directional Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    private SpriteRenderer spriteRenderer;
    private Vector2 lastDirection = Vector2.down; // Default facing down

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set initial sprite to down direction
        if (spriteDown != null)
        {
            spriteRenderer.sprite = spriteDown;
        }
    }

    /// <summary>
    /// Updates the player sprite based on movement direction.
    /// Call this from PlayerMovement with the current input vector.
    /// </summary>
    public void UpdateSprite(Vector2 movementDirection)
    {
        // Only update direction if there's actual movement input
        if (movementDirection != Vector2.zero)
        {
            lastDirection = movementDirection;
        }

        // Determine which sprite to show based on direction
        // Prioritize vertical movement (up/down) over horizontal when moving diagonally
        if (lastDirection.y > 0) // Moving up
        {
            if (spriteUp != null)
                spriteRenderer.sprite = spriteUp;
        }
        else if (lastDirection.y < 0) // Moving down
        {
            if (spriteDown != null)
                spriteRenderer.sprite = spriteDown;
        }
        else if (lastDirection.x < 0) // Moving left
        {
            if (spriteLeft != null)
                spriteRenderer.sprite = spriteLeft;
        }
        else if (lastDirection.x > 0) // Moving right
        {
            if (spriteRight != null)
                spriteRenderer.sprite = spriteRight;
        }
    }
}
