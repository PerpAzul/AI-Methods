using UnityEngine;
using UnityEngine.SceneManagement;

public class TuerTiefensuche : InteractableI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    protected override void Interact()
    {
        if (!VariableStore.IsLobbyTutorialFinished()) {
            return;
        }

        int curLevel = VariableStore.GetCurrentLevelSearch();
        //currentLevel = ist das Level, was man als n�chstes machen muss
        switch (curLevel)
        {
            case 0:
                SceneManager.LoadScene("00-Tiefensuche");
                break;
            case 1:
                SceneManager.LoadScene("01-Tiefensuche");
                break;
            case 2:
                SceneManager.LoadScene("02-Tiefensuche");
                break;
            case 3:
                SceneManager.LoadScene("03-Tiefensuche");
                break;
            default:
                break;
        }

    }
}
