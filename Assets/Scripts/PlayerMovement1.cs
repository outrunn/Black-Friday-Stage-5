using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;            // Units per second

    private Rigidbody2D rb;
    private Vector2 input;           // raw input each frame
    private PlayerSpriteController spriteController;

    public int lives;
    [SerializeField] private TextMeshProUGUI livesText;

    //Damage Stuff
    private SpriteRenderer spriteRenderer;
    private Color damageColor = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;        // top-down 2D
        rb.freezeRotation = true;    // no rotation needed with sprite switching
        lives = 3;
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        spriteController = GetComponent<PlayerSpriteController>();
    }

    private void Update()
    {
        //lock player movement if game is not playing
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentState != GameState.Playing && GameStateManager.Instance.CurrentState != GameState.Tutorial) { return; }

        // Read input (WASD/Arrow Keys by default Unity axes)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Restrict to 4 directions only (no diagonals)
        // Prioritize vertical movement over horizontal when both are pressed
        if (y != 0)
        {
            input = new Vector2(0, y); // Only vertical movement
        }
        else if (x != 0)
        {
            input = new Vector2(x, 0); // Only horizontal movement
        }
        else
        {
            input = Vector2.zero; // No movement
        }

        // Update sprite direction based on movement input
        if (spriteController != null)
        {
            spriteController.UpdateSprite(input);
        }

        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    private void FixedUpdate()
    {
        // --- Movement ---
        // Input is already restricted to 4 directions in Update(), no normalization needed
        rb.linearVelocity = input * moveSpeed;
    }

    public void TakeDamage()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = originalColor;
    }


}
