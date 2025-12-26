using UnityEngine;

public class ToyController : MonoBehaviour
{
    public static ToyController Instance;
    private bool isToyHeld = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetIsToyHeld(bool value)
    {
        isToyHeld = value;
    }

    public bool GetIsToyHeld()
    {
        return isToyHeld;
    }
}
