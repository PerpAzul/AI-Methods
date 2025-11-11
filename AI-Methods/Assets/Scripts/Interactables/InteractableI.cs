using UnityEngine;

public class InteractableI : MonoBehaviour
{
    public string promptMessage;
    public bool useEvents; //Add or remove an InteractionEvent component to this gameobject 
    
    //This function will be called by our player
    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }
    
    protected virtual void Interact()
    {
        //We won't have any code written in this function
        //This is a template function to be overridden by our subclasses
    }
}
