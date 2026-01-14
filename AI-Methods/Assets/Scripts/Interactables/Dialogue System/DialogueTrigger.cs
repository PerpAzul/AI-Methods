using UnityEngine;

public class DialogueTrigger : InteractableI
{
    public Dialogue dialogue;
    [SerializeField] private DialogueManager manager;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject arrow;
    private bool hasInteracted = false;

    protected override void Interact()
    {
        alreadyInteracted = true;
        // for decision trees bot
        if (arrow != null)
        {
            if (!hasInteracted)
            {
                hasInteracted = true;
                arrow.SetActive(false);
            }
        }
        if (pauseMenu.isPaused)
        {
            return;
        }
        
        if (manager.isInDialogue)
        {
            manager.DisplayNextSentence();
        }
        else
        {
            manager.StartDialogue(dialogue);   
        }
    }
}
