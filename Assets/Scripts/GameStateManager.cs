using System.Collections;
using System.Collections.Generic;
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

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameHudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI collectiblesText;
    [SerializeField] private TextMeshProUGUI countDownText;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform mainSpawn;

    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    private TimerUI timer;

    public GameState CurrentState { get; private set; }

    public int numOfCollectibles = 0;
    public int numOfTutorialCollectibles = 0;

    // PowerUps
    private GameObject[] powerUps;

    [Header("LEVEL SELECTOR")]
    [SerializeField] private int currentLevel = 1;

    [Header("LEVEL 1 SETTINGS")]
    [SerializeField] private GameObject[] level1Keys;
    [SerializeField] private Transform[] level1SpawnPoints;
    [SerializeField] private int level1KeyCount = 2;

    [Header("LEVEL 2 SETTINGS")]
    [SerializeField] private GameObject[] level2Keys;
    [SerializeField] private Transform[] level2SpawnPoints;
    [SerializeField] private int level2KeyCount = 4;

    // Track last spawn index per key (by level)
    private Dictionary<GameObject, int> lastSpawn_L1 = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> lastSpawn_L2 = new Dictionary<GameObject, int>();

    private bool keysSpawnedThisRun = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        timer = timerText.GetComponent<TimerUI>();

        powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        // Initialize per-level tracking
        foreach (var k in level1Keys) lastSpawn_L1[k] = -1;
        foreach (var k in level2Keys) lastSpawn_L2[k] = -1;

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


    public void UpdateUI()
    {
        mainMenuPanel.SetActive(false);
        introPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gameHudPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        countDownPanel.SetActive(false);

        switch (CurrentState)
        {
            case GameState.Start:
                mainMenuPanel.SetActive(true);

                keysSpawnedThisRun = false;

                numOfCollectibles = 0;
                collectiblesText.text = "0 / 2";
                break;

            case GameState.Intro:
                introPanel.SetActive(true);
                dialogueRunner.StartDialogue("IntroScript");
                infoPanel.SetActive(true);
                break;

            case GameState.Playing:
                gameHudPanel.SetActive(true);
                infoPanel.SetActive(false);

                timer.timerIsRunning = true;

                player.GetComponent<PlayerMovement>().lives = 3;

                RespawnAllPowerUps();

                if (!keysSpawnedThisRun)
                {
                    SpawnKeysForCurrentLevel();
                    keysSpawnedThisRun = true;
                }
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
        yield return new WaitForSeconds(1);

        countDownText.text = "2";
        yield return new WaitForSeconds(1);

        countDownText.text = "1";
        yield return new WaitForSeconds(1);

        countDownPanel.SetActive(false);

        SetState(GameState.Playing);
    }


    public void RespawnAllPowerUps()
    {
        foreach (GameObject p in powerUps)
            if (p) p.SetActive(true);
    }


    private void SpawnKeysForCurrentLevel()
    {
        if (currentLevel == 1)
            SpawnKeys(level1Keys, level1SpawnPoints, level1KeyCount, lastSpawn_L1);
        else
            SpawnKeys(level2Keys, level2SpawnPoints, level2KeyCount, lastSpawn_L2);
    }


    private void SpawnKeys(GameObject[] keys, Transform[] spawnPoints, int keyCount, Dictionary<GameObject, int> lastSpawnTable)
    {
        keyCount = Mathf.Min(keyCount, keys.Length, spawnPoints.Length);

        foreach (GameObject k in keys)
            k.SetActive(false);

        List<int> available = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
            available.Add(i);

        // Remove last spawn index for each key
        foreach (var key in keys)
        {
            int lastIndex = lastSpawnTable[key];
            if (lastIndex != -1 && available.Contains(lastIndex))
                available.Remove(lastIndex);
        }

        // Spawn keys
        for (int i = 0; i < keyCount; i++)
        {
            GameObject key = keys[i];

            int r = Random.Range(0, available.Count);
            int chosenSpot = available[r];

            key.transform.position = spawnPoints[chosenSpot].position;
            key.SetActive(true);

            lastSpawnTable[key] = chosenSpot;

            available.RemoveAt(r);
        }
    }
}
