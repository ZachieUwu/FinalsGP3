using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour, IInteractable
{
    [TextArea(3, 5)]
    public string[] dialogueLines;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines);
    }

}
