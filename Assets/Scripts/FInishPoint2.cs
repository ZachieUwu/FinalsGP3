using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FInishPoint2 : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private string specialCutscene;
    [SerializeField] bool isPlayerNear = false;
    private SItemMatch secretInventory;
    public AudioManager audioManager;

    private void Start()
    {
        secretInventory = FindObjectOfType<SItemMatch>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.BothSecretItemsCollected())
            {
                SceneController.instance.NextLevel(specialCutscene);
            }
            else
            {
                SceneController.instance.NextLevel(nextScene);
            }

            audioManager.StopMusic();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}