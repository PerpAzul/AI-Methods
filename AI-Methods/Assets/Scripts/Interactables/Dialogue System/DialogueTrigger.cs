using UnityEngine;

public class DialogueTrigger : InteractableI
{
    public Dialogue dialogue;
    [SerializeField] private DialogueManager manager;

    protected override void Interact()
    {
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
