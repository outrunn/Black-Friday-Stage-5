using UnityEngine;
<<<<<<< HEAD
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
        //timerIsRunning = true;
    }



    void Update()
    {
        //turn on timer when gamestate is playing
        if (GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            timerIsRunning = true;
        }
        else
        {
            timerIsRunning = false;
        }
=======
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
>>>>>>> 2fb781afb7095444b0904b757a77b8e05de97613
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

<<<<<<< HEAD
                // Start blinking when 1 minute or less remains
=======
                // Start blinking when only 1 minute remains
>>>>>>> 2fb781afb7095444b0904b757a77b8e05de97613
                if (timeRemaining <= 60 && !isBlinking)
                {
                    StartCoroutine(BlinkText());
                    isBlinking = true;
                }
            }
            else
            {
<<<<<<< HEAD
                // Stop timer
                timeRemaining = 0;
                timerIsRunning = false;
                StopAllCoroutines();
                timeText.color = normalColor; // Reset to normal color
                Debug.Log("Timer finished!");
=======
                Debug.Log("Timer finished!");
                timeRemaining = 0;
                timerIsRunning = false;
                timeText.color = normalColor; // reset to normal color
>>>>>>> 2fb781afb7095444b0904b757a77b8e05de97613
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
<<<<<<< HEAD
        // Clamp to zero
=======
>>>>>>> 2fb781afb7095444b0904b757a77b8e05de97613
        timeToDisplay = Mathf.Max(timeToDisplay, 0f);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

<<<<<<< HEAD
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
=======
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
>>>>>>> 2fb781afb7095444b0904b757a77b8e05de97613
    }
}
