using UnityEngine;

public class Puzzle2Controller : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzle2Points;
    [SerializeField] private GameObject[] toys;
    private Toy[] toyScripts;

    private void Start()
    {
        toyScripts = new Toy[toys.Length];
        for (int i = 0; i < toys.Length; i++)
        {
            toyScripts[i] = toys[i].GetComponent<Toy>();
            if (toyScripts[i] == null)
            {
                Debug.LogError($"Toy script not found on toy at index {i}");
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.Puzzle1)
        {
            if (CheckPuzzle())
            {
                Debug.Log("Puzzle 1 completed!");
                //GameManager.UpdateGameState(GameManager.GameState.Puzzle2);
                GameManager.UpdateGameState(GameManager.GameState.Puzzle4); // Şimdilik
            }
        }
    }

    private bool CheckPuzzle()
    {
        foreach (var toy in toyScripts)
        {
            if (!toy.IsCorrectlyPlaced())
            {
                return false;
            }
        }
        return true;
    }


}
