using UnityEngine;
using UnityEngine.UI;

public class EvaluateDecisionTree : MonoBehaviour
{
    public enum Result
    {
        Undefined,
        Good,
        Bad
    }
    
    public Canvas decisionTreeCanvas;
    public Material materialOutput;

    public bool metallic;
    public bool dangerous;
    public bool blueEnergy;

    public Result Evaluate()
    {
        if (FindNodeText("1_1").Equals("?"))
        {
            Debug.Log("Characteristics: Metallic = " + metallic + ", Dangerous = " + dangerous + ", Blue Energy = " + blueEnergy);
            return Result.Undefined;
        }

        if (AnswerNode("1_1"))
        {
            if (FindNodeText("2_1") == "?")
            {
                return Result.Undefined;
            }

            if (AnswerNode("2_1"))
            {
                if (FindNodeText("3_1") == "?")
                {
                    return Result.Undefined;
                }

                if (AnswerNode("3_1"))
                {
                    return GetResultForToggle(1);
                }
                else
                {
                    return GetResultForToggle(2);
                }
            }
            else
            {
                if (FindNodeText("3_2") == "?")
                {
                    return Result.Undefined;
                }

                if (AnswerNode("3_2"))
                {
                    return GetResultForToggle(3);
                }
                else
                {
                    return GetResultForToggle(4);
                }
                
            }
        }
        else
        {
            if (FindNodeText("2_2") == "?")
            {
                return Result.Undefined;
            }

            if (AnswerNode("2_2"))
            {
                if (FindNodeText("3_3") == "?")
                {
                    return Result.Undefined;
                }

                if (AnswerNode("3_3"))
                {
                    return GetResultForToggle(5);
                }
                else
                {
                    return GetResultForToggle(6);
                }
            }
            else
            {
                if (FindNodeText("3_4") == "?")
                {
                    return Result.Undefined;
                }

                if (AnswerNode("3_4"))
                {
                    return GetResultForToggle(7);
                }
                else
                {
                    return GetResultForToggle(8);
                }
                
            }
        }
    }

    private Result GetResultForToggle(int number)
    {
        foreach (Button button in decisionTreeCanvas.GetComponentsInChildren<Button>())
        {
            if (button.name == $"toggle_{number}")
            {
                if (button.GetComponent<ToggleButton>().isOn)
                {
                    Debug.Log("Good");
                    return Result.Good;
                }
                Debug.Log("Bad");
                return Result.Bad;
            }
        }
        Debug.Log("Weird");
        return Result.Undefined;
    }

    private string FindNodeText(string key)
    {
        foreach (Text t in decisionTreeCanvas.GetComponentsInChildren<Text>())
        {
            if (t.name.Contains(key))
            {
                return t.text;
            }
        }

        return null;
    }

    private bool AnswerNode(string key)
    {
        string text = FindNodeText(key);
        if (text == "Metall?" && metallic || text == "Schädlich?" && dangerous ||
            text == "Blaue Energie?" && blueEnergy)
        {
            return true;
        }

        return false;
    }
    
    
    private bool isPlayerNear;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(Evaluate());
        }
    }
}
