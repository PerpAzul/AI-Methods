using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToNormal : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject cutscene1;
    [SerializeField] private GameObject cutscene2;
    [SerializeField] private GameObject cutscene3;
    [SerializeField] private GameObject dtInterface;
    public void finishLevel()
    {
        Debug.Log("Click");
        VariableStore.SetScoreDecision(0, 0);
        VariableStore.MarkLevelAsFinishedDecision(0);
        StartCoroutine(LoadSceneRoutine("DecisionTreeSmall"));
        progressBar.SetActive(false);
        dtInterface.SetActive(false);
        cutscene3.GetComponent<Canvas>().enabled = false;
    }

    public void goToCutscene1()
    {
        levelComplete.SetActive(false);
        cutscene2.SetActive(false);
        cutscene1.SetActive(true);
    }

    public void goToCutscene2()
    {
        cutscene1.SetActive(false);
        cutscene3.SetActive(false);
        cutscene2.SetActive(true);
    }

    public void goToCutscene3()
    {
        cutscene2.SetActive(false);
        cutscene3.SetActive(true);
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
