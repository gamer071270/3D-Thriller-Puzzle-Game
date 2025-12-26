using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{   
    public void PlayGame()
    {
        GameManager.UpdateGameState(GameManager.GameState.GameStart);
    }

    public void Options()
    {
        Debug.Log("Options menu opened.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
