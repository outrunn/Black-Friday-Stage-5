using UnityEngine;

public enum GameState
{
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
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject gameHudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    //Player
    [SerializeField] private GameObject player;

    //Integers
    public int numOfCollectibles = 0;


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

        SetState(GameState.Tutorial); //default state
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        UpdateUI();
    }

    //Updates the UI according to the CurrentState
    public void UpdateUI()
    {
        //Turn off all panels
        tutorialPanel.SetActive(false);
        gameHudPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        //checks what the current state is and turns on the corresponding canvas panal
        switch (CurrentState)
        {
            case GameState.Playing:
                gameHudPanel.SetActive(true);
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                break;
            case GameState.Victory:
                winPanel.SetActive(true);
                break;
            case GameState.Tutorial:
                tutorialPanel.SetActive(true);
                break;
        }
    }
}
