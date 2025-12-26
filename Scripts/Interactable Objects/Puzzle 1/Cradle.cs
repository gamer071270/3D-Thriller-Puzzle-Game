using UnityEngine;
using System.Collections;

public class Cradle : MonoBehaviour, IInteractable
{
    private UIManager _uiManager;
    private ObjectManager _objectManager;
    private float swingAngle = 20.0f;
    private int swingCount = 3;
    private float swingDuration = 3.0f;
    private bool hasInteracted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uiManager = UIManager.Instance;
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance is null");
        }

        _objectManager = ObjectManager.Instance;
        if (_objectManager == null)
        {
            Debug.LogError("ObjectManager instance is null");
        }
    }

    public void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;
        GameManager.UpdateGameState(GameManager.GameState.Puzzle1);
        StartCoroutine(SwingCradle(transform, swingAngle, swingCount, swingDuration));
    }

    public void ShowInteractionPrompt()
    {
        if (hasInteracted) return;
        _uiManager.PressEInteractable("Press E to rock the cradle", true);
    }

    public void HideInteractionPrompt()
    {
        _uiManager.PressEInteractable("", false);
    }

    private IEnumerator SwingCradle(Transform cradlePivot, float swingAngle, int swings, float swingDuration)
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < swings; i++)
        {
            yield return RotateOverTime(cradlePivot, -swingAngle, swingAngle, swingDuration / 2f);
            yield return RotateOverTime(cradlePivot, swingAngle, -swingAngle, swingDuration / 2f);
        }

        cradlePivot.localRotation = Quaternion.identity;
        _objectManager.DropPhoto1();
    }

    private IEnumerator RotateOverTime(Transform t, float fromZ, float toZ, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float z = Mathf.Lerp(fromZ, toZ, time / duration);
            t.localRotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }
        t.localRotation = Quaternion.Euler(0, 0, toZ);
    }

}
