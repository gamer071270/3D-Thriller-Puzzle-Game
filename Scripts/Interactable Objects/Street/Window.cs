using UnityEngine;
using System;

public class Window : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    private bool isActive = false;

    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Street)
        {
            isActive = true;
        }
    }

    public void Interact()
    {
        if (isActive)
        {
            string[] dialogues = new string[]
            {
                "I think I saw something... or someone inside.",
                "I better need to find a way in."
            };
            _uiManager.ShowMultipleDialogues(dialogues, 3f);
            isActive = false;
        }
    }

    public void ShowInteractionPrompt()
    {
        if (isActive)
        {
            _uiManager.PressEInteractable("Press E to look", true);
        }
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }
}
