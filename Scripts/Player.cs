using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float mouseSensitivity = 6.0f;
    [SerializeField] private float upDownRange = 80.0f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float interactionDistance = 3.0f;

    [Header("References")]
    [SerializeField] private Transform targetPosition; // Puzzle 1 için hedef pozisyon

    private UIManager _uiManager;
    private IInteractable currentInteractable;
    private CharacterController controller;
    private Camera mainCamera;

    private bool canMove = true;
    private float mouseYRotation = 0.0f;
    private Vector3 currentMovement = Vector3.zero;
    private bool hasShownDialogue = false;

    private void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is not found.");
        }
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing from the Player GameObject.");
        }
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found in the scene.");
        }
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        transform.position = new Vector3(-130, 1, -90);
        //transform.position = new Vector3(105, 1, 120);
    }

    private void Update()
    {
        if (!canMove) return;
        Movement();
        RotateView();
        CheckPlayerCamera();
        CheckStreetSegmentCount();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        UIManager.OnImageShown += DisableMovement;
        UIManager.OnImageHidden += EnableMovement;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        UIManager.OnImageShown -= DisableMovement;
        UIManager.OnImageHidden -= EnableMovement;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.GameStart:
                canMove = true;
                transform.position = new Vector3(-130, 1, -90);
                //transform.position = new Vector3(105, 1, 120);
                break;
            case GameManager.GameState.Walking:
                break;
            case GameManager.GameState.Street:
                _uiManager.ShowDialogue("Wait a minute! This is the same trash can and door I just passed.", 3f);
                break;
            case GameManager.GameState.Building:
                break;
            case GameManager.GameState.Puzzle1:
                HandlePuzzle1();
                break;
            default:
                break;
        }
    }

    private void DisableMovement()
    {
        canMove = false;
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    private void Movement()
    {
        if (!controller.enabled) return;
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1.0f;
        float verticalSpeed = Input.GetAxis("Vertical") * speed * speedMultiplier;
        float horizontalSpeed = Input.GetAxis("Horizontal") * speed * speedMultiplier;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        Jumping();
        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        controller.Move(currentMovement * Time.deltaTime);
    }

    private void RotateView()
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);


        mouseYRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseYRotation = Mathf.Clamp(mouseYRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(mouseYRotation, 0, 0);
    }

    private void Jumping()
    {
        if (controller.isGrounded)
        {
            currentMovement.y = -0.5f; // Reset vertical movement when grounded

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentMovement.y = jumpForce; // Apply jump force
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime; // Apply gravity when not grounded
        }
    }

    private void CheckStreetSegmentCount()
    {
        if (transform.position.x > 60.0f && !hasShownDialogue)
        {
            hasShownDialogue = true;
            GameManager.UpdateGameState(GameManager.GameState.Street);
        }
    }

    private void CheckPlayerCamera()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable?.HideInteractionPrompt();
                    interactable.ShowInteractionPrompt();
                    currentInteractable = interactable;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }
        else if (currentInteractable != null)
        {
            currentInteractable.HideInteractionPrompt();
            currentInteractable = null;
        }
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        StartCoroutine(TeleportCoroutine(newPosition));
    }

    private IEnumerator TeleportCoroutine(Vector3 newPosition)
    {
        controller.enabled = false;
        transform.position = newPosition;
        yield return null;
        controller.enabled = true;
    }

    private void HandlePuzzle1()
    {
        StartCoroutine(MoveToTargetAndReturn(targetPosition, 1f));
    }

    private IEnumerator MoveToTargetAndReturn(Transform target, float moveDuration)
    {
        canMove = false;
        Quaternion originalRotation = transform.rotation;
        Vector3 startPos = transform.position;

        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;

        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(originalRotation, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;


        yield return new WaitForSeconds(9.0f); // beşiği sallama süresi

        time = 0f;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            transform.position = Vector3.Lerp(targetPos, startPos, t);
            transform.rotation = Quaternion.Slerp(targetRot, originalRotation, t);
            yield return null;
        }

        transform.position = startPos;
        transform.rotation = originalRotation;
        canMove = true;
    }


}