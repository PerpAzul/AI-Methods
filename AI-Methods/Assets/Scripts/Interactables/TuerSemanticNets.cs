using UnityEngine;
using UnityEngine.SceneManagement;

public class TuerSemanticNets : InteractableI
{
    string earlierMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (VariableStore.IsLobbyTutorialFinished()) {
            promptMessage = "Semantische Netze";
        }
        earlierMessage = promptMessage;
    }

    public void ChangePrompt()
    {
        promptMessage = "Semantische Netze";
    }

    // Update is called once per frame
    protected override void Interact()
    {
        if (!VariableStore.IsLobbyTutorialFinished()) {
            return;
        }

        int curLevel = VariableStore.GetCurrentLevelSemantic();
        //currentLevel = ist das Level, was man als n�chstes machen muss
        switch (curLevel)
        {
            case 0:
                SceneManager.LoadScene("SemanticNets0");
                break;
            case 1:
                SceneManager.LoadScene("SemanticNets1");
                break;
            case 2:
                SceneManager.LoadScene("SemanticNets2");
                break;
            default:
                promptMessage = earlierMessage + "\n<size=70%>Alle Level geschafft! Gehe durch eine andere Tür oder in die Mitte der Lobby um bestimmte Level nochmal zu spielen.</size>";
                break;
        }

    }

}
