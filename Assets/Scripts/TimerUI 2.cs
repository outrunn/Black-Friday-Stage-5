using UnityEngine;
using TMPro;
using System.Collections;

public class TimerUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 300f;  // 5 minutes in seconds
    public bool timerIsRunning = false;

    [Header("UI Settings")]
    public TextMeshProUGUI timeText;    // Drag your UI text here in Inspector
    public Color normalColor = Color.white;  // Default text color
    public Color warningColor = Color.red;   // Blinking color

    private bool isBlinking = false;

    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                // Start blinking when 1 minute or less remains
                if (timeRemaining <= 60 && !isBlinking)
                {
                    StartCoroutine(BlinkText());
                    isBlinking = true;
                }
            }
            else
            {
                // Stop timer
                timeRemaining = 0;
                timerIsRunning = false;
                StopAllCoroutines();
                timeText.color = normalColor; // Reset to normal color
                Debug.Log("Timer finished!");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Clamp to zero
        timeToDisplay = Mathf.Max(timeToDisplay, 0f);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the UI text
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator BlinkText()
    {
        while (timeRemaining > 0 && timeRemaining <= 60)
        {
            timeText.color = warningColor;
            yield return new WaitForSeconds(0.5f);
            timeText.color = normalColor;
            yield return new WaitForSeconds(0.5f);
        }

        // When done blinking (timer ended or reset)
        timeText.color = normalColor;
    }
}
