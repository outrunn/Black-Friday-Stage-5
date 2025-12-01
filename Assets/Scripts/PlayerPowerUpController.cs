using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerPowerUpController : MonoBehaviour
{
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;

    [Header("Speed Power-Up")]
    public float speedMultiplier = 1.7f;

    [Header("UI Text")]
    public TextMeshProUGUI powerUpText;   // Assign in Inspector

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // -----------------------------
    // PUBLIC FUNCTIONS CALLED BY POWERUP PICKUPS
    // -----------------------------

    public void ApplySpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostRoutine(duration));
    }

    public void ApplyHeart()
    {
        movement.AddLife(1);

        // Heart is instant → show message for 2 seconds
        if (powerUpText != null)
            StartCoroutine(ShowTemporaryMessage("Extra Life!", 2f));
    }

    public void ApplyInvisibility(float duration)
    {
        StartCoroutine(InvisibilityRoutine(duration));
    }

    // -----------------------------
    // POWER-UP ROUTINES
    // -----------------------------

    private IEnumerator SpeedBoostRoutine(float duration)
    {
        float originalSpeed = movement.GetMoveSpeed();
        movement.SetMoveSpeed(originalSpeed * speedMultiplier);

        // Show message
        if (powerUpText != null)
            powerUpText.text = "Speed Boost!";

        // Wait for the REAL effect duration
        yield return new WaitForSeconds(duration);

        // Reset speed
        movement.SetMoveSpeed(originalSpeed);

        // Clear message
        if (powerUpText != null)
            powerUpText.text = "";
    }

    private IEnumerator InvisibilityRoutine(float duration)
    {
        // Activate effect
        EnemyVisionController.PlayerInvisible = true;

        // Optional transparency
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);

        // Show message
        if (powerUpText != null)
            powerUpText.text = "Invisibility!";

        yield return new WaitForSeconds(duration);

        // Effect ends
        EnemyVisionController.PlayerInvisible = false;

        // Restore sprite opacity
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // Clear message
        if (powerUpText != null)
            powerUpText.text = "";
    }

    // Instant-only power-up temporary message (used by Heart)
    private IEnumerator ShowTemporaryMessage(string message, float time)
    {
        if (powerUpText != null)
        {
            powerUpText.text = message;
            yield return new WaitForSeconds(time);
            powerUpText.text = "";
        }
    }
}
