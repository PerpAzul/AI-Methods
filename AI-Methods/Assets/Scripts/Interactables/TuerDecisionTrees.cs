using UnityEngine;
using UnityEngine.SceneManagement;

public class TuerDecisionTrees : InteractableI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Interact()
    {
        int curLevel = VariableStore.GetCurrentLevelDecision();
        //currentLevel = ist das Level, was man als nächstes machen muss
        switch (curLevel)
        {
            case 0:
                SceneManager.LoadScene("DecisionTreeObst");
                break;
            case 1:
                SceneManager.LoadScene("DecisionTreeSmall");
                break;
            default:
                break;
        }

    }
}
