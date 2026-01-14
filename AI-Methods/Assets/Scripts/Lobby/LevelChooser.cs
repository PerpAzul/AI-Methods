using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{
    public void Search0() {
        if (VariableStore.GetCurrentLevelSearch() > 0) {
            StartCoroutine(LoadSceneRoutine("00-Tiefensuche"));
        }
    }


    public void Search1() {
        if (VariableStore.GetCurrentLevelSearch() > 1) {
            StartCoroutine(LoadSceneRoutine("01-Tiefensuche"));
        }
    }

    public void Search2() {
        if (VariableStore.GetCurrentLevelSearch() > 2) {
            StartCoroutine(LoadSceneRoutine("02-Tiefensuche"));
        }
    }

    public void Search3() {
        if (VariableStore.GetCurrentLevelSearch() > 3) {
            StartCoroutine(LoadSceneRoutine("03-Tiefensuche"));
        }
    }

    public void Semantic0() {
        Debug.Log(VariableStore.GetCurrentLevelSemantic());
        if (VariableStore.GetCurrentLevelSemantic() > 0) {
            Debug.Log("Here");
            StartCoroutine(LoadSceneRoutine("SemanticNets0"));
        }
    }

    public void Semantic1() {
        if (VariableStore.GetCurrentLevelSemantic() > 1) {
            StartCoroutine(LoadSceneRoutine("SemanticNets1"));
        }
    }

    public void Semantic2() {
        if (VariableStore.GetCurrentLevelSemantic() > 2) {
            StartCoroutine(LoadSceneRoutine("SemanticNets2"));
        }
    }

    public void Decision0() {
        if (VariableStore.GetCurrentLevelDecision() > 0) {
            StartCoroutine(LoadSceneRoutine("DecisionTreeObst"));
        }
    }

    public void Decision1() {
        if (VariableStore.GetCurrentLevelDecision() > 1) {
            StartCoroutine(LoadSceneRoutine("DecisionTreeSmall"));
        }
    }


    private IEnumerator LoadSceneRoutine(string nextSceneName)
    {
        yield return null;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
