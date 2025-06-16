using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxItems = 20;

    public bool AddItem(Item newItem)
    {
        if (items.Count >= maxItems)
        {
            Debug.Log("Inventario lleno");
            return false;
        }

        items.Add(newItem);
        Debug.Log($"Item añadido: {newItem.itemName}");
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Item removido: {item.itemName}");
        }
    }
}