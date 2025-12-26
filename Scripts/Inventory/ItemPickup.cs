using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;

    public void Interact()
    {
        if (Inventory.Instance.AddItem(item))
        {
            UIManager.Instance.ShowImage(item.icon);
            Destroy(gameObject);
        }
    }

    public void ShowInteractionPrompt()
    {
        UIManager.Instance.PressEInteractable("Press E to pick up " + item.itemName, true);
    }

    public void HideInteractionPrompt()
    {
        UIManager.Instance.PressEInteractable("", false);
    }
}
