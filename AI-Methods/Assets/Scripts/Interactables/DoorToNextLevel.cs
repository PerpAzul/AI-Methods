using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : InteractableI
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private string nextSceneName;
    
    protected override void Interact()
    {
        if (pauseMenu.isPaused)
        {
            return;
        }
        
        SceneManager.LoadScene(nextSceneName);
    }
}
