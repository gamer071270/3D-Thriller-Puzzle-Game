using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FuseBoxManager : MonoBehaviour
{
    [SerializeField] private List<Appliance> appliances;  // Tüm cihazlar
    [SerializeField] private int maxCapacity = 100;        // Sigortanın kapasitesi
    [SerializeField] private float fuseIncrease = 1f;   // Sigorta yükünü artırma hızı
    [SerializeField] private float puzzleDuration = 180f; // Kaç saniye dayanmalı (örn: 3 dk)
    [SerializeField] private Text uiText; // UI Text bileşeni
    
    public static FuseBoxManager Instance;
    public static event System.Action OnPuzzle4Success;
    public static event System.Action<string> OnPuzzle4Fail;
    private int openApplianceCount;
    private float totalLoad;
    private float puzzleTimer;
    private bool failed;

    private void Awake()
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

    public void StartPuzzle4()
    {
        puzzleTimer = 0f;
        totalLoad = 0f;
        failed = false;
        openApplianceCount = 0;
        foreach (var appliance in appliances)
        {
            appliance.Initialize();
            switch (appliance.applianceID)
            {
                case 0: // Fridge
                    StartCoroutine(appliance.Fridge());
                    break;
                case 1: // Air Conditioner
                    StartCoroutine(appliance.AirConditioner());
                    break;
                default:
                    StartCoroutine(appliance.OneCycle());
                    break;
            }
        }
        StartCoroutine(HandlePuzzle4());
    }

    private IEnumerator HandlePuzzle4()
    {
        while (true)
        {
            UpdateApplianceTimer();
            CalculateTotalLoad();
            CheckFail();
            puzzleTimer += 1f;
            if (puzzleTimer >= puzzleDuration)
            {
                PuzzleCompleted();
                break;
            }
            if (failed) break;
            ShowStatus();
            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateApplianceCondition(int applianceID, bool isOpen)
    {
        appliances[applianceID].SetIsOn(isOpen);
    }

    public void UpdateOpenApplianceCount(bool openStatus)
    {
        if (openStatus) openApplianceCount--;
        else openApplianceCount++;
    }

    private void UpdateApplianceTimer()
    {
        foreach (var appliance in appliances)
        {
            appliance.UpdateTimer(1f);
        }
    }
    
    private void ShowStatus()
    {
        uiText.text = Mathf.Round(puzzleTimer) + "s / " + puzzleDuration + "s\n";
        uiText.text += $"Open appliance number: {openApplianceCount}\n";
        uiText.text += $"Total Load: {Mathf.Round(totalLoad)}/{maxCapacity}\n";
        foreach (var appliance in appliances)
        {
            uiText.text += appliance.GetStatus() + "\n";
        }
    }

    private void CheckFail()
    {
        foreach (var appliance in appliances)
        {
            if (appliance.GetFail())
            {
                PuzzleFailed($"Appliance {appliance.applianceID} failure!");
            }
        }
    }

    private void CalculateTotalLoad()
    {
        switch (openApplianceCount)
        {
            case 0:
                totalLoad -= fuseIncrease;
                break;
            case 1:
                totalLoad -= fuseIncrease * 0.5f;
                break;
            case 2:
                totalLoad += 0;
                break;
            case 3:
                totalLoad += fuseIncrease * 0.5f;
                break;
            case 4:
                totalLoad += fuseIncrease;
                break;
            case 5:
                totalLoad += fuseIncrease * 2f;
                break;
            case 6:
                totalLoad += fuseIncrease * 3f;
                break;
            case 7:
                totalLoad += fuseIncrease * 4f;
                break;
            default:
                break;
        }
        if (totalLoad < 0) totalLoad = 0;
        if (totalLoad > maxCapacity)
        {
            PuzzleFailed("Fuse overloaded!");
        }
    }

    private void PuzzleFailed(string reason)
    {
        failed = true;
        OnPuzzle4Fail?.Invoke(reason);
        // Puzzle başarısız oldu, gerekli işlemleri yap
    }

    private void PuzzleCompleted()
    {
        OnPuzzle4Success?.Invoke();
        // Puzzle başarıyla tamamlandı, gerekli işlemleri yap
    }
}
