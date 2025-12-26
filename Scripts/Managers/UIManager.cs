using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text")]
    [SerializeField] private Text interactableText;
    [SerializeField] private Text dialogueText;

    [Header("Images")]
    [SerializeField] private Image image;

    public static event Action OnImageShown;
    public static event Action OnImageHidden;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameManager.OnGameStateChanged += ClearAllUI;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= ClearAllUI;
    }


    public void ClearAllUI(GameManager.GameState newState)
    {
        interactableText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }

    public void PressEInteractable(string prompt, bool show)
    {
        interactableText.text = prompt;
        interactableText.gameObject.SetActive(show);
    }

    public void ShowDialogue(string message, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message, duration));
    }

    public void ShowMultipleDialogues(string[] messages, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMultipleRoutine(messages, duration));
    }

    private IEnumerator ShowRoutine(string msg, float duration)
    {
        dialogueText.text = msg;
        dialogueText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        dialogueText.gameObject.SetActive(false);
    }

    private IEnumerator ShowMultipleRoutine(string[] msgs, float duration)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            dialogueText.text = msgs[i];
            dialogueText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            dialogueText.gameObject.SetActive(false);
        }

    }

    public void ShowImage(Sprite sprite)
    {
        if (sprite == null) return;
        StopAllCoroutines();
        StartCoroutine(HideImageAfterDelay(sprite, 3f));
    }

    private IEnumerator HideImageAfterDelay(Sprite sprite, float delay)
    {
        OnImageShown?.Invoke();
        image.sprite = sprite;
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        image.gameObject.SetActive(false);
        OnImageHidden?.Invoke();
    }
}
