using UnityEngine;
using System;

public class DoorKey : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    private bool isActive = false;
    public static event Action OnDoorKeyPickedUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            _uiManager.ShowDialogue("This might belong to the door.", 2f);
            OnDoorKeyPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
        

    public void ShowInteractionPrompt()
    {
        if (isActive)
        {
            _uiManager.PressEInteractable("Press E to pick up", true);
        }
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }
}