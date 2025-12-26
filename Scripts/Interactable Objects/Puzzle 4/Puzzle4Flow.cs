using UnityEngine;

public class Puzzle4Flow : MonoBehaviour
{
    [SerializeField] private FuseBoxManager fuseBox;
    
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        FuseBoxManager.OnPuzzle4Success += EndPuzzleSuccess;
        FuseBoxManager.OnPuzzle4Fail += EndPuzzleFail;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        FuseBoxManager.OnPuzzle4Success -= EndPuzzleSuccess;
        FuseBoxManager.OnPuzzle4Fail -= EndPuzzleFail;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Puzzle4)
        {
            if (fuseBox == null)
            {
                Debug.LogError("FuseBoxManager instance is not found.");
            }
            else
            {
                fuseBox.StartPuzzle4();
            }
        }
    }

    private void EndPuzzleSuccess()
    {
        Debug.Log("Puzzle Completed Successfully!");
        GameManager.UpdateGameState(GameManager.GameState.Puzzle5);
    }

    private void EndPuzzleFail(string reason)
    {
        Debug.Log("Puzzle Failed: " + reason);
    }
}
