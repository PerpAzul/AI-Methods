using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : InteractableI
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private PlayerMovement player;
    
    protected override void Interact()
    {
        if (pauseMenu.isPaused || player.isLoading)
        {
            return;
        }

        player.isLoading = true;
        StartCoroutine(LoadSceneRoutine(nextSceneName));
    }
    
    private System.Collections.IEnumerator LoadSceneRoutine(string nextSceneName)
    {
        // set last player position for respawn
        PlayerPositionMemory.lastPosition = player.transform.position;

        loadingScreen.SetActive(true);
        yield return null;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        //operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
