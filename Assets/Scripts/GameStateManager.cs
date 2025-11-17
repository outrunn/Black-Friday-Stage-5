using TMPro;
using UnityEngine;
using Yarn.Unity;

public enum GameState
{
    Start,
    Intro,
    Tutorial,
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
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameHudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    //Text
    [SerializeField] private TextMeshProUGUI collectiblesText;

    //Player
    [SerializeField] private GameObject player;

    [SerializeField] private DialogueRunner dialogueRunner;

    //Integers
    public int numOfCollectibles = 0;
    public int numOfTutorialCollectibles = 0;


    public GameState CurrentState { get; private set; }

    private void Awake()
    {

        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject); //keeps it alive across scenes

        SetState(GameState.Start); //default state
        mainMenuPanel.SetActive(true);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        UpdateUI();
    }

    public void goToIntro()
    {
        CurrentState = GameState.Intro;
        UpdateUI();
    }

    //Updates the UI according to the CurrentState
    public void UpdateUI()
    {
        //Turn off all panels
        mainMenuPanel.SetActive(false);
        introPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gameHudPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        //checks what the current state is and turns on the corresponding canvas panal
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
                player.GetComponent<PlayerMovement>().lives = 3;
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                break;
            case GameState.Victory:
                winPanel.SetActive(true);
                break;
            case GameState.Tutorial:
                tutorialPanel.SetActive(true);
                infoPanel.SetActive(true);
                player.GetComponent<PlayerMovement>().lives = 3;
                break;
        }
    }
}
