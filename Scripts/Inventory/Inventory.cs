using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> items = new List<Item>();
    public int maxSize = 10;

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

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSize)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        items.Add(item);
        Debug.Log("Added: " + item.itemName);
        return true;
    }
}
