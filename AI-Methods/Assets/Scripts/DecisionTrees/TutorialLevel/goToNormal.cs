using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToNormal : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject levelComplete;
    public void finishLevel()
    {
        Debug.Log("Click");
        VariableStore.SetScoreDecision(0, 0);
        VariableStore.MarkLevelAsFinishedDecision(0);
        StartCoroutine(LoadSceneRoutine("DecisionTreeSmall"));
        progressBar.SetActive(false);
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
