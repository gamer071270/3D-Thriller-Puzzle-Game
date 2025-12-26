using UnityEngine;
using System.Collections;

public class Drawer : MonoBehaviour, IInteractable
{

    private UIManager _uiManager;
    [SerializeField] private int drawerID;
    private Vector3 openPosition;
    private Vector3 closedPosition;
    private Coroutine _currentCoroutine;
    private float speed = 2f;
    private bool isOpen = false;
    private bool canOpen = false;

    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
        if (drawerID == 0)
        {
            openPosition = transform.localPosition + new Vector3(0.775f, 0, 0);
            closedPosition = transform.localPosition;
        }
        else if (drawerID == 1)
        {
            openPosition = transform.localPosition + new Vector3(-0.775f, 0, 0);
            closedPosition = transform.localPosition;
        }
        else
        {
            Debug.LogError("Invalid drawerID. Please set it to 0 or 1.");
        }
        
    }

    private void OnEnable()
    {
        WardrobeController.OnBothDoorsOpened += OpenDrawer;
        WardrobeController.OnBothDoorsClosed += CloseDrawer;
    }

    private void OnDisable()
    {
        WardrobeController.OnBothDoorsOpened -= OpenDrawer;
        WardrobeController.OnBothDoorsClosed -= CloseDrawer;
    }

    private void OpenDrawer()
    {
        canOpen = true;
    }

    private void CloseDrawer()
    {
        canOpen = false;
    }

    public void Interact()
    {
        if (drawerID == 0)
        {
            MoveDrawer();
        }
        else
        {
            if (canOpen)
            {
                MoveDrawer();
            }
        }
    }

    public void ShowInteractionPrompt()
    {
        if (drawerID == 0)
        {
            if (isOpen)
                _uiManager.PressEInteractable("Press E to close", true);
            else
                _uiManager.PressEInteractable("Press E to open", true);
        }
        else
        {
            if (canOpen)
            {
                if (isOpen)
                    _uiManager.PressEInteractable("Press E to close", true);
                else
                    _uiManager.PressEInteractable("Press E to open", true);
            }
        }
        
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    private void MoveDrawer()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(ToggleDrawer());
        ShowInteractionPrompt();
    }

    private IEnumerator ToggleDrawer()
    {
        Vector3 target = isOpen ? closedPosition : openPosition;
        isOpen = !isOpen;
        Vector3 start = transform.localPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}
