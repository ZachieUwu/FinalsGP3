using UnityEngine;
using UnityEngine.Playables;

public class DialogueController : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool waitingForInput = false;

    public void WaitForInput()
    {
        timeline.Pause(); // Pause the Timeline
        waitingForInput = true;
    }

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.E))
        {
            waitingForInput = false; // Reset waiting state
            timeline.Play(); // Resume Timeline
        }
    }
}
