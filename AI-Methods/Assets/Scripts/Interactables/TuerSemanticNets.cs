using UnityEngine;
using UnityEngine.SceneManagement;

public class TuerSemanticNets : InteractableI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Interact()
    {
        int curLevel = VariableStore.GetCurrentLevelSemantic();
        //currentLevel = ist das Level, was man als nächstes machen muss
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
                break;
        }

    }

}
