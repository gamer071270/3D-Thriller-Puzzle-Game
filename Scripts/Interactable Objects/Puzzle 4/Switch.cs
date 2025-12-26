using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private int switchID;
    private FuseBoxManager fuseBox;
    private Vector3 closedRotation = new Vector3(0, 0, 15);
    private Vector3 openRotation = new Vector3(0, 0, -15);
    private float speed = 2f;
    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        fuseBox = FuseBoxManager.Instance;
        if (fuseBox == null)
        {
            Debug.LogError("FuseBoxManager instance is not found.");
        }
    }

    public void Interact()
    {
        if (!isMoving)
        {
            fuseBox.UpdateOpenApplianceCount(isOpen);
            StartCoroutine(ToggleSwitch(isOpen ? closedRotation : openRotation));
            fuseBox.UpdateApplianceCondition(switchID, isOpen);
        }
    }

    public void ShowInteractionPrompt(){}

    public void HideInteractionPrompt(){}

    private IEnumerator ToggleSwitch(Vector3 targetRot)
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
