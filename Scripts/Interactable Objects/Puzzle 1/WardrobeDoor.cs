using UnityEngine;
using System;
using System.Collections;

public class WardrobeDoor : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    [SerializeField] private int doorID; // 0 for left door, 1 for right door
    private WardrobeController _wardrobeController;
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private Coroutine _currentCoroutine;
    private float speed = 2f;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
        _wardrobeController = FindFirstObjectByType<WardrobeController>();
        if (_wardrobeController == null)
        {
            Debug.LogError("WardrobeController not found in the scene.");
        }
        if (doorID == 0)
        {
            closedRotation = transform.localEulerAngles;
            openRotation = closedRotation + new Vector3(0, 90f, 0);
        }
        else if (doorID == 1 || doorID == 2)
        {
            closedRotation = transform.localEulerAngles;
            openRotation = closedRotation + new Vector3(0, -90f, 0);
        }
        else
        {
            Debug.LogError("Invalid doorID. Please set it to 0 or 1.");
        }
    }

    public void Interact()
    {
        ToggleDoor();
        ShowInteractionPrompt();
        _wardrobeController.SetDoorState(doorID, isOpen);
    }

    public void ShowInteractionPrompt()
    {
        if(isOpen)
            _uiManager.PressEInteractable("Press E to close", true);
        else
        _uiManager.PressEInteractable("Press E to open", true);
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    public void ToggleDoor()
    {
        if (!isMoving)
            StartCoroutine(RotateDoor(isOpen ? closedRotation : openRotation));
    }

    private IEnumerator RotateDoor(Vector3 targetRot)
    {
        isMoving = true;
        isOpen = !isOpen;
        Quaternion startRot = transform.localRotation;
        Quaternion target = Quaternion.Euler(targetRot);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Slerp(startRot, target, t);
            yield return null;
        }
        isMoving = false;
    }
}
