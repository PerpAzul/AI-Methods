using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToNextLevel : MonoBehaviour
{
    private ProgressBar progressBar;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject levelComplete;
    private void Start()
    {
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();
    }
    public void finishLevel()
    {
        Debug.Log("Click");
        VariableStore.SetScoreDecision(progressBar.points, 1);
        VariableStore.MarkLevelAsFinishedDecision(1);
        StartCoroutine(LoadSceneRoutine("Lobby German"));
        progressBar.gameObject.SetActive(false);
        levelComplete.GetComponent<Canvas>().enabled = false;
    }

    // Loading Screen for Scene from semantic nets 
    private IEnumerator LoadSceneRoutine(string nextSceneName)
    {
        loadingScreen.SetActive(true);
        yield return null;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
