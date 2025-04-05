using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> collectedItems = new List<GameObject>();
    public int maxItems;

    public void SetInteractableItem(GameObject item)
    {
        if (item != null && collectedItems.Count < maxItems)
        {
            collectedItems.Add(item);
            item.SetActive(false);
        }
    }
}