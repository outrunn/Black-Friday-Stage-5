using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    public enum PowerType { Speed, Invisibility, Heart }

    [Header("Random Power-Up")]
    public bool randomizeOnPickup = true;

    public PowerType type;
    public float duration = 5f;

    [Header("UI Reference")]
    public TextMeshProUGUI powerUpText; // drag same UI text here

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPowerUpController p = other.GetComponent<PlayerPowerUpController>();
        if (p == null) return;

        // RANDOMIZE TYPE ON PICKUP
        if (randomizeOnPickup)
        {
            int randomIndex = Random.Range(0, 3);
            type = (PowerType)randomIndex;
        }

        // APPLY THE SELECTED POWER-UP
        switch (type)
        {
            case PowerType.Speed:
                p.ApplySpeedBoost(duration);
                break;

            case PowerType.Invisibility:
                p.ApplyInvisibility(duration);
                break;

            case PowerType.Heart:
                p.ApplyHeart();
                break;
        }

        gameObject.SetActive(false);
    }
}
