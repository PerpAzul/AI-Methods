using UnityEngine;

public class Teleporter : InteractableI
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject teleport;
    [SerializeField] private GameObject player;
    
    protected override void Interact()
    {
        if (pauseMenu.isPaused)
        {
            return;
        }
        
        var controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.position = teleport.transform.position;
        controller.enabled = true;
    }
}
