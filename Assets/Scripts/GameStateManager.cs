using System.Collections;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public enum GameState
{
    Start,
    Intro,
    Tutorial,
    CountDown,
    Playing,
    GameOver,
    Victory
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    //Panels
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameHudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    //Text
    [SerializeField] private TextMeshProUGUI collectiblesText;
    [SerializeField] private TextMeshProUGUI countDownText;

    //Player
    [SerializeField] private GameObject player;
    [SerializeField] private Transform mainSpawn;

    [SerializeField] private DialogueRunner dialogueRunner;

    //Integers
    public int numOfCollectibles = 0;
    public int numOfTutorialCollectibles = 0;

    [SerializeField] private TextMeshProUGUI timerText;
    private TimerUI timer;

    public GameState CurrentState { get; private set; }

    // ------------------------------------------------------
    // NEW: Power-ups stored here
    // ------------------------------------------------------
    private GameObject[] powerUps;

    // ------------------------------------------------------
    // NEW: Key spawn system
    // ------------------------------------------------------
    [Header("Key Spawn")]
    [SerializeField] private GameObject[] keyItems;        // your 2 keys
    [SerializeField] private Transform[] keySpawnPoints;   // all spawn spots


    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        timer = timerText.GetComponent<TimerUI>();
        if (timer == null)
        {
            Debug.LogError("Timer script not found");
        }

        // ------------------------------------------------------
        // NEW: Cache all power-ups in the scene
        // ------------------------------------------------------
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        SetState(GameState.Start);
        mainMenuPanel.SetActive(true);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) &&
            Input.GetKeyDown(KeyCode.LeftShift) &&
            Input.GetKeyDown(KeyCode.Q))
        {
            player.SetActive(true);
            SetState(GameState.CountDown);
        }
    }

    public void startCountDown()
    {
        SetState(GameState.CountDown);
    }

    public void goToIntro()
    {
        SetState(GameState.Intro);
    }

    // Updates the UI according to the CurrentState
    public void UpdateUI()
    {
        //Turn off all panels
        mainMenuPanel.SetActive(false);
        introPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gameHudPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        countDownPanel.SetActive(false);

        switch (CurrentState)
        {
            case GameState.Intro:
                introPanel.SetActive(true);
                dialogueRunner.StartDialogue("IntroScript");
                infoPanel.SetActive(true);
                break;

            case GameState.Playing:
                gameHudPanel.SetActive(true);
                infoPanel.SetActive(false);
                timer.timerIsRunning = true;
                timer.timeRemaining = 300f;
                player.GetComponent<PlayerMovement>().lives = 3;

                // ------------------------------------------------------
                // NEW: respawn all power-ups on restart
                // ------------------------------------------------------
                RespawnAllPowerUps();

                // ------------------------------------------------------
                // NEW: spawn keys randomly at game start
                // ------------------------------------------------------
                SpawnKeysRandomly();

                break;

            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                timer.timerIsRunning = false;
                break;

            case GameState.Victory:
                winPanel.SetActive(true);
                Time.timeScale = 0f;
                break;

            case GameState.Tutorial:
                tutorialPanel.SetActive(true);
                infoPanel.SetActive(true);
                player.GetComponent<PlayerMovement>().lives = 2000000;
                break;

            case GameState.CountDown:
                player.transform.position = mainSpawn.position;
                dialogueRunner.Stop();
                StartCoroutine(startGame());
                break;
        }
    }

    IEnumerator startGame()
    {
        countDownPanel.SetActive(true);
        countDownText.text = "3";

        yield return new WaitForSeconds(1f);
        countDownText.text = "2";

        yield return new WaitForSeconds(1f);
        countDownText.text = "1";

        yield return new WaitForSeconds(1f);
        countDownPanel.SetActive(false);

        SetState(GameState.Playing);
    }

    // ------------------------------------------------------
    // NEW: Reactivate every power-up that was collected
    // ------------------------------------------------------
    public void RespawnAllPowerUps()
    {
        foreach (GameObject p in powerUps)
        {
            if (p != null)
                p.SetActive(true);
        }
    }

    // ------------------------------------------------------
    // NEW: Randomize 2 keys without overlapping
    // ------------------------------------------------------
    public void SpawnKeysRandomly()
    {
        if (keyItems.Length == 0 || keySpawnPoints.Length == 0)
            return;

        // create list of available spawn indexes
        System.Collections.Generic.List<int> available = new System.Collections.Generic.List<int>();

        for (int i = 0; i < keySpawnPoints.Length; i++)
            available.Add(i);

        // randomize each key separately
        foreach (GameObject key in keyItems)
        {
            if (key == null) continue;

            int index = Random.Range(0, available.Count);
            int chosenSpot = available[index];

            key.transform.position = keySpawnPoints[chosenSpot].position;
            key.SetActive(true);

            available.RemoveAt(index); // prevents overlap
        }
    }
}
