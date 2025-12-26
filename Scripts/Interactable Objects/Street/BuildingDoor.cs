using UnityEngine;

public class BuildingDoor : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    private ScreenFader _screenFader;
    private Player player;
    [SerializeField] private bool hasKey;
    private bool stateUpdated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
        _screenFader = ScreenFader.Instance;
        if (_screenFader == null)
        {
            Debug.LogError("ScreenFader instance is not found.");
        }
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found");
        }
    }

    private void OnEnable()
    {
        DoorKey.OnDoorKeyPickedUp += HandleDoorKeyPickedUp;
    }

    private void OnDisable()
    {
        DoorKey.OnDoorKeyPickedUp -= HandleDoorKeyPickedUp;
    }

    private void HandleDoorKeyPickedUp()
    {
        _uiManager.ShowDialogue("This might belong to the door.", 3f);
        hasKey = true;
    }

    public void Interact()
    {
        if (hasKey)
        {
            if (!stateUpdated)
            {
                GameManager.UpdateGameState(GameManager.GameState.Building);
                stateUpdated = true;
            }

            _screenFader.FadeIn();
            if (!PlayerInBuilding())
            {
                player.TeleportPlayer(new Vector3(100, 1, 102)); // Binanın içine ışınlanma
            }
            else
            {
                player.TeleportPlayer(new Vector3(0, 1, 0)); // Sokağa ışınlanma
            }
            _screenFader.FadeOut();
        }
        else
        {
            _uiManager.ShowDialogue("The door is locked.", 2f);
        }
    }

    public void ShowInteractionPrompt()
    {
        if (hasKey)
        {
            _uiManager.PressEInteractable("Press E to open", true);
        }
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    private bool PlayerInBuilding()
    {
        return player.transform.position.z > 100;
    }
}