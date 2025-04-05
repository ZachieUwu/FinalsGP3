using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool firstCharacterCollected = false;
    private bool secondCharacterCollected = false;

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

    public void SetSecretItemCollected(bool isSecondCharacter)
    {
        if (isSecondCharacter)
        {
            secondCharacterCollected = true;
        }
        else
        {
            firstCharacterCollected = true;
        }
    }

    public bool BothSecretItemsCollected()
    {
        return firstCharacterCollected && secondCharacterCollected;
    }

    internal void SetSecretItemCollected(object isSecondCharacter)
    {
        throw new NotImplementedException();
    }
}
