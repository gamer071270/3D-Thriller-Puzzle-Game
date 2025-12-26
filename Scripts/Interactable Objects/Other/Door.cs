using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private GameObject meshColliderGameObject;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;
    private UIManager _uiManager;


    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    public void Interact()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(ToggleDoor());
        ShowInteractionPrompt();
    }

    public void ShowInteractionPrompt()
    {
        string doorPrompt = isOpen ? "Press E to close" : "Press E to open";
        _uiManager.PressEInteractable(doorPrompt, true);
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(doorPivot.rotation, targetRotation) > 0.01f)
        {
            doorPivot.rotation = Quaternion.Lerp(doorPivot.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        doorPivot.rotation = targetRotation;
    }
}