using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    
    public void BaseInteract()
    {
        Interact();
    }

    public void BaseDestroy()
    {
        Destroy();
    }
    
    protected virtual void Interact()
    {

    }

    protected virtual void Destroy()
    {
        
    }
}
