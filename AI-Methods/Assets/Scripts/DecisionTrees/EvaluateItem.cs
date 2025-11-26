using System;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateItem : MonoBehaviour
{
    
    private enum Result
    {
        Undefined,
        Good,
        Bad
    }
    
    [Header("Decision Tree Canvas")] public GameObject decisionTree;

    [Header("Game Manager")] public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickup>() != null)
        {
            Pickup pickup = other.GetComponent<Pickup>();
            Debug.Log($"Evaluating: Metal: {pickup.isMetal}, Danger: {pickup.isDangerous}, Blue Energy: {pickup.isBlueEnergy}");
            Result result = Evaluate(pickup.isMetal, pickup.isDangerous, pickup.isBlueEnergy);
            switch (result)
            {
                case Result.Good:
                    break;
            }
        }
    }

    private bool metallic, dangerous, blueEnergy;
    private Result Evaluate(bool isMetallic, bool isDangerous, bool hasBlueEnergy)
    {
        metallic = isMetallic;
        dangerous = isDangerous;
        blueEnergy = hasBlueEnergy;
        if (FindNodeText("1_1").Equals("?"))
        {
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
        foreach (Button button in decisionTree.GetComponentsInChildren<Button>())
        {
            if (button.name == $"toggle_{number}")
            {
                if (button.GetComponent<ToggleButton>().isOn)
                {
                    return Result.Good;
                }
                return Result.Bad;
            }
        }
        Debug.Log("Weird");
        return Result.Undefined;
    }

    private string FindNodeText(string key)
    {
        foreach (Text t in decisionTree.GetComponentsInChildren<Text>())
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
    
}
