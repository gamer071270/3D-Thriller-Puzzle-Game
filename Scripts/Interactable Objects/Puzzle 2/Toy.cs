using UnityEngine;

public class Toy : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    private ToyController _toyController;
    [SerializeField] private Transform handPoint;
    private Rigidbody rb;
    private bool isHeld = false;
    private bool isCorrectlyPlaced = false;

    private void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }
        _toyController = ToyController.Instance;
        if (_toyController == null)
        {
            Debug.LogError("ToyController instance is null");
        }
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
            isHeld = false;
            _toyController.SetIsToyHeld(isHeld);
        }
    }

    public void Interact()
    {
        if (!_toyController.GetIsToyHeld())
        {
            PickUp();
            isHeld = true;
            _toyController.SetIsToyHeld(isHeld);
        }
    }

    public void ShowInteractionPrompt()
    {
        _uiManager.PressEInteractable("Press E to pick up", true);
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    private void PickUp()
    {
        transform.SetParent(handPoint);
        transform.localPosition = Vector3.zero;
        rb.isKinematic = true;
    }

    private void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(Camera.main.transform.forward * 0.1f, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ToyTriggerPoint"))
        {
            isCorrectlyPlaced = true;
        }
        else
        {
            isCorrectlyPlaced = false;
        }
    }

    public bool IsCorrectlyPlaced()
    {
        return isCorrectlyPlaced;
    }

}
