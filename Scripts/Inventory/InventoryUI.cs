using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private Text inventoryText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void RefreshUI()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in Inventory.Instance.items)
        {
            sb.AppendLine(item.itemName);
        }
        inventoryText.text = sb.ToString();
    }
}
