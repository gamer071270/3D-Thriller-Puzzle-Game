using UnityEngine;
using System;

public class WardrobeController : MonoBehaviour
{
    public bool door1Opened;
    public bool door2Opened;
    public static event Action OnBothDoorsOpened;
    public static event Action OnBothDoorsClosed;

    public void SetDoorState(int doorID, bool isOpen)
    {
        if (doorID == 0) door1Opened = isOpen;
        else if (doorID == 1) door2Opened = isOpen;

        if (door1Opened && door2Opened)
            OnBothDoorsOpened?.Invoke();
        else
            OnBothDoorsClosed?.Invoke();
    }
}
