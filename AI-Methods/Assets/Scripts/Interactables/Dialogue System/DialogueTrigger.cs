using UnityEngine;

public class DialogueTrigger : InteractableI
{
    public Dialogue dialogue;
    [SerializeField] private DialogueManager manager;
    [SerializeField] private PauseMenu pauseMenu;

    protected override void Interact()
    {
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
