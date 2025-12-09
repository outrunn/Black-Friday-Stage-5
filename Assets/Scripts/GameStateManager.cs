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

    // ------------------------------------------------------
    // UI PANELS
    // ------------------------------------------------------
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameHudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    // ------------------------------------------------------
    // UI TEXT
    // ------------------------------------------------------
    [SerializeField] private TextMeshProUGUI collectiblesText;
    [SerializeField] private TextMeshProUGUI countDownText;

    // ------------------------------------------------------
    // PLAYER + SPAWN
    // ------------------------------------------------------
    [SerializeField] private GameObject player;
    [SerializeField] private Transform mainSpawn;

    [SerializeField] private DialogueRunner dialogueRunner;

    // ------------------------------------------------------
    // TIMER
    // ------------------------------------------------------
    [SerializeField] private TextMeshProUGUI timerText;
    private TimerUI timer;


    // ------------------------------------------------------
    // GAME STATE
    // ------------------------------------------------------
    public GameState CurrentState { get; private set; }

    // ------------------------------------------------------
    // COLLECTIBLE COUNTERS (needed by Collectible.cs)
    // ------------------------------------------------------
    public int numOfCollectibles = 0;
    public int numOfTutorialCollectibles = 0;

    // ------------------------------------------------------
    // POWER-UPS
    // ------------------------------------------------------
    private GameObject[] powerUps;

    // ------------------------------------------------------
    // LEVEL SYSTEM
    // ------------------------------------------------------
    [Header("Level Selector")]
    [SerializeField] private int currentLevel = 1; // 1 or 2

    // ---------------- LEVEL 1 ----------------
    [Header("LEVEL 1 SETTINGS")]
    [SerializeField] private GameObject[] level1Keys;        // keys only for level 1
    [SerializeField] private Transform[] level1SpawnPoints;  // spawn positions for level 1
    [SerializeField] private int level1KeyCount = 2;         // keys to spawn in level 1

    // ---------------- LEVEL 2 ----------------
    [Header("LEVEL 2 SETTINGS")]
    [SerializeField] private GameObject[] level2Keys;        // keys only for level 2
    [SerializeField] private Transform[] level2SpawnPoints;  // spawn positions for level 2
    [SerializeField] private int level2KeyCount = 4;         // keys to spawn in level 2



    private void Awake()
    {
        // Singleton init
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Timer link
        timer = timerText.GetComponent<TimerUI>();
        if (timer == null)
            Debug.LogError("TimerUI script missing from Timer Text.");

        // Cache power-ups in scene
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        SetState(GameState.Start);
        mainMenuPanel.SetActive(true);
    }



    // ------------------------------------------------------
    // STATE CONTROL
    // ------------------------------------------------------
    public void SetState(GameState newState)
    {
        CurrentState = newState;
        UpdateUI();
    }

    void Update()
    {
        // Quick-start cheat
        if (Input.GetKey(KeyCode.LeftControl) &&
            Input.GetKey(KeyCode.LeftShift) &&
            Input.GetKeyDown(KeyCode.Q))
        {
            player.SetActive(true);
            SetState(GameState.CountDown);
        }
    }

    public void startCountDown() => SetState(GameState.CountDown);
    public void goToIntro() => SetState(GameState.Intro);



    // ------------------------------------------------------
    // UI + STATE HANDLING
    // ------------------------------------------------------
    public void UpdateUI()
    {
        //Turn off all panels first
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

                RespawnAllPowerUps();
                SpawnKeysForCurrentLevel();
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
    // POWER-UPS RESET
    // ------------------------------------------------------
    public void RespawnAllPowerUps()
    {
        foreach (GameObject p in powerUps)
            if (p != null)
                p.SetActive(true);
    }



    // ------------------------------------------------------
    // LEVEL KEY SPAWNING
    // ------------------------------------------------------
    private void SpawnKeysForCurrentLevel()
    {
        if (currentLevel == 1)
        {
            SpawnKeys(level1Keys, level1SpawnPoints, level1KeyCount);
        }
        else
        {
            SpawnKeys(level2Keys, level2SpawnPoints, level2KeyCount);
        }
    }

    private void SpawnKeys(GameObject[] keys, Transform[] spawnPoints, int keyCount)
    {
        if (keys.Length == 0 || spawnPoints.Length == 0 || keyCount <= 0)
            return;

        // Clamp keyCount
        keyCount = Mathf.Min(keyCount, keys.Length);
        keyCount = Mathf.Min(keyCount, spawnPoints.Length);

        // Turn off ALL keys in this level
        foreach (GameObject k in keys)
            k.SetActive(false);

        // Build list of available spawn indexes
        System.Collections.Generic.List<int> available = new System.Collections.Generic.List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
            available.Add(i);

        // Spawn correct number of keys
        for (int i = 0; i < keyCount; i++)
        {
            int r = Random.Range(0, available.Count);
            int chosenSpot = available[r];

            keys[i].transform.position = spawnPoints[chosenSpot].position;
            keys[i].SetActive(true);

            available.RemoveAt(r);
        }
    }
}
