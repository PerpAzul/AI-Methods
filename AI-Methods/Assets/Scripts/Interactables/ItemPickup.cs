using UnityEngine;

public class ItemPickup : InteractableI
{
    [SerializeField] private PauseMenu pauseMenu;
    
    protected override void Interact()
    {
        if (pauseMenu.isPaused)
        {
            return;
        }
        
        Destroy(gameObject);
    }
}
