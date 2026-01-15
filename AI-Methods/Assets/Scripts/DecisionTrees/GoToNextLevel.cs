using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToNextLevel : MonoBehaviour
{
    [SerializeField] private GameObject cutscene1;
    [SerializeField] private GameObject cutscene2;
    [SerializeField] private GameObject cutscene3;
    [SerializeField] private GameObject cutscene4;
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
        progressBar.transform.root.gameObject.SetActive(false);
        cutscene4.GetComponent<Canvas>().enabled = false;
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
        cutscene4.SetActive(false);
        cutscene3.SetActive(true);
    }

    public void goToCutscene4()
    {
        cutscene3.SetActive(false);
        cutscene4.SetActive(true);
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
