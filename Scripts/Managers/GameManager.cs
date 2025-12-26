using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Opening,
        GameStart,
        Walking,
        Street,
        Building,
        Puzzle1,
        Puzzle2,
        Puzzle3,
        Puzzle4,
        Puzzle5,
        Puzzle6
    }


    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;
    private GameState currentGameState;
    [SerializeField] private GameObject playerPrefab;
    private GameObject playerInstance;
    private ScreenFader _screenFader;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _screenFader = ScreenFader.Instance;
        if (_screenFader == null)
        {
            Debug.LogError("ScreenFader instance is not found.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentGameState == GameState.GameStart)
        {
            //SpawnPlayer();
        }
    }

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public static void UpdateGameState(GameState newState)
    {
        Instance.currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
        switch (newState)
        {
            case GameState.Opening:
                HandleOpening();
                break;
            case GameState.GameStart:
                HandleGameStart();
                break;
            case GameState.Walking:
                HandleWalking();
                break;
            case GameState.Street:
                HandleStreet();
                break;
            case GameState.Building:
                HandleBuilding();
                break;
            case GameState.Puzzle1:
                // HandlePuzzle1();
                break;
            case GameState.Puzzle2:
                // HandlePuzzle2();
                break;
            case GameState.Puzzle3:
                // HandlePuzzle3();
                break;
            case GameState.Puzzle4:
                // HandlePuzzle4();
                break;
            case GameState.Puzzle5:
                // HandlePuzzle5();
                break;
            case GameState.Puzzle6:
                // HandlePuzzle6();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private static void HandleOpening()
    {
        /*
        Handle the opening state:
        Logo display
        Opening the start menu
        Main menu, options selection
        Creating/continuing a game
        */
        //SceneManager.LoadScene(0);
    }

    private static void HandleGameStart()
    {
        /*
        Handle the game start state:
        Initialize the game
        Loading the first scene
        Setting up the player position
        */
        Instance._screenFader.FadeIn();
        SceneManager.LoadScene(1);
        Instance._screenFader.FadeOut();
    }

    private static void HandleWalking()
    {
        /*
        Handle the walking state:
        Enable player movement
        Play walking animation
        Player walking sound

        Player starts in the house, can explore the surroundings.
        Phone call sound, the player answers it.
        A friend calls, asking her to come over (dialogues and sounds)
        The player decides to go out, puts on her shoes and leaves the house.
        Walks in the street, looking around.
        After a while, the player discovers an alley (trigger event).
        */
    }

    private static void HandleStreet()
    {

    }

    private static void HandleBuilding()
    {
    }

    private void SpawnPlayer()
    {
        if (playerInstance != null) return;

        Vector3 spawnPoint = new Vector3(-130f, 1f, -90f);
        playerInstance = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        DontDestroyOnLoad(playerInstance);
    }

}