using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SItemMatch : MonoBehaviour
{
    public List<GameObject> collectedSecretItems = new List<GameObject>();
    public int maxSecretItems = 1;

    public void AddSecretItem(GameObject secretItem)
    {
        if (collectedSecretItems.Count < maxSecretItems)
        {
            collectedSecretItems.Add(secretItem);
            secretItem.SetActive(false);
        }
    }

    public bool HasAllSecretItems()
    {
        return collectedSecretItems.Count == maxSecretItems;
    }
}
