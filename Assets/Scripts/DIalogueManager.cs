using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    private string[] dialogueLines;
    private int currentLine;
    private bool isTyping;
    public float dialogueDisappearTime = 2f; // Time before dialogue disappears

    private void Awake()
    {
        if (Instance == null) Instance = this;
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLine = 0;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeSentence(dialogueLines[currentLine]));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        isTyping = false;

        if (currentLine == dialogueLines.Length - 1)
        {
            StartCoroutine(HideDialogueAfterSeconds());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueBox.activeSelf && !isTyping)
        {
            NextLine();
        }
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeSentence(dialogueLines[currentLine]));
        }
    }

    private IEnumerator HideDialogueAfterSeconds()
    {
        yield return new WaitForSeconds(dialogueDisappearTime);
        dialogueBox.SetActive(false);
    }
}
