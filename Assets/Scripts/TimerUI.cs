using UnityEngine;
using TMPro;  // Needed if you are using TextMeshPro

public class TimerUI : MonoBehaviour
{
    public float timeRemaining = 300f;  // 5 minutes in seconds
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;    // Drag your UI text here in Inspector

    private bool isBlinking = false;
    public Color normalColor = Color.white;  // default text color
    public Color warningColor = Color.red;   // blinking warning color

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

                // Start blinking when only 1 minute remains
                if (timeRemaining <= 60 && !isBlinking)
                {
                    StartCoroutine(BlinkText());
                    isBlinking = true;
                }
            }
            else
            {
                Debug.Log("Timer finished!");
                timeRemaining = 0;
                timerIsRunning = false;
                timeText.color = normalColor; // reset to normal color
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(timeToDisplay, 0f);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    System.Collections.IEnumerator BlinkText()
    {
        while (timeRemaining > 0) // keep blinking until timer ends
        {
            timeText.color = warningColor;  // turn red
            yield return new WaitForSeconds(0.5f);
            timeText.color = normalColor;   // back to white
            yield return new WaitForSeconds(0.5f);
        }
    }
}
