using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DecisionTrees;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EvaluateItem : MonoBehaviour
{
    
    private class Result
    {
        public List<string> Path;
        public bool Useful;

        public Result(List<string> path, bool useful)
        {
            Path = path;
            Useful = useful;
        }
    }
    
    [Header("Decision Tree Canvas")] public GameObject decisionTree;
    [Header("Database")] public Database database;
    public ProgressBar progressBar;
    public NPCGuide guide;

    public RectTransform warning;


    private IEnumerator TestDatabase()
    {
        if(guide) guide.ContinueIfCurrentActionEquals("first_test");
        if(guide) guide.ContinueIfCurrentActionEquals("second_test");
        warning.gameObject.SetActive(false);
        float progress = 0;
        foreach (Item item in database.ScannedItems)
        {
            Result result = Evaluate(item.IsMetal, item.IsDangerous, item.HasBlueEnergy);
            HighlightPath(result.Path);
            database.DisplayEvaluate(item, progress/database.ScannedItems.Count);
            yield return new WaitForSeconds(1);

            if (guide && result.Path[^1] == "toggle_1")
            {
                guide.ContinueIfCurrentActionEquals("third_test");
            }
            
            if (!result.Path[^1].StartsWith("toggle"))
            {
                DisplayPathWarning(result.Path[^1]);
                database.DisplayResult("?", progress/database.ScannedItems.Count);
                break;
            }
            else if(result.Useful == item.Useful())
            {
                if (item.IsDangerous && item.HasBlueEnergy && item.IsMetal)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("machine_t_correct");
                }
                progress++;
                progressBar.curr = progress;
                database.DisplayResult("KORREKT", progress/database.ScannedItems.Count);
                yield return new WaitForSeconds(1);
            }
            else
            {
                database.DisplayResult("FALSCH", progress/database.ScannedItems.Count);
                DisplayResultError(result.Path[^1]);
                break;
            }
        }
    }

    private void Start()
    {
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(TestDatabase());
        }
        
    }

    private void HighlightPath(List<string> path)
    {
        foreach (Image image in decisionTree.GetComponentsInChildren<Image>())
        {
            if (image.name.StartsWith("dropzone"))
            {
                image.color = path.Any(item => image.name == $"dropzone_{item}") ? new Color(1f, 1f, 1f, 1f) : new Color32(0, 0, 255, 150);
            }

            if (image.name.StartsWith("yes") || image.name.StartsWith("no"))
            {
                image.color = path.Contains(image.name) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.1f);
            }
        }
    }

    private void DisplayPathWarning(string where)
    {
        Transform parent = GameObject.Find($"dropzone_{where}").transform;
        warning.SetParent(parent);
        warning.anchoredPosition = new Vector2(0, 50);
        warning.localRotation = Quaternion.identity;
        warning.localScale = Vector3.one;
        warning.gameObject.SetActive(true);
    }

    private void DisplayResultError(string where)
    {
        Transform parent = GameObject.Find(where).transform;
        warning.SetParent(parent);
        warning.anchoredPosition = new Vector2(100, -45);
        warning.localRotation = Quaternion.identity;
        warning.localScale = Vector3.one;
        warning.gameObject.SetActive(true);
    }

    private bool metallic, dangerous, blueEnergy;
    private Result Evaluate(bool isMetallic, bool isDangerous, bool hasBlueEnergy)
    {
        List<string> path = new();
        metallic = isMetallic;
        dangerous = isDangerous;
        blueEnergy = hasBlueEnergy;
        path.Add("1_1");
        if (FindNodeText("1_1").Equals("?"))
        {
            return new Result(path, false);
        }

        if (AnswerNode("1_1"))
        {
            path.Add("yes_1");
            path.Add("2_1");
            if (FindNodeText("2_1") == "?")
            {
                return new Result(path, false);
            }

            if (AnswerNode("2_1"))
            {
                path.Add("yes_2_1");
                path.Add("3_1");
                if (FindNodeText("3_1") == "?")
                {
                    return new Result(path, false);
                }

                if (AnswerNode("3_1"))
                {
                    path.Add("yes_3_1");
                    path.Add("toggle_1");
                    return new Result(path, GetResultForToggle(1));
                }
                else
                {
                    path.Add("no_3_1");
                    path.Add("toggle_2");
                    return new Result(path, GetResultForToggle(2));
                }
            }
            else
            {
                path.Add("no_2_1");
                path.Add("3_2");
                if (FindNodeText("3_2") == "?")
                {
                    return new Result(path, false);
                }

                if (AnswerNode("3_2"))
                {
                    path.Add("yes_3_2");
                    path.Add("toggle_3");
                    return new Result(path, GetResultForToggle(3));
                }
                else
                {
                    path.Add("no_3_2");
                    path.Add("toggle_4");
                    return new Result(path, GetResultForToggle(4));
                }
                
            }
        }
        else
        {
            path.Add("no_1");
            path.Add("2_2");
            if (FindNodeText("2_2") == "?")
            {
                return new Result(path, false);
            }

            if (AnswerNode("2_2"))
            {
                path.Add("yes_2_2");
                path.Add("3_3");
                if (FindNodeText("3_3") == "?")
                {
                    return new Result(path, false);
                }

                if (AnswerNode("3_3"))
                {
                    path.Add("yes_3_3");
                    path.Add("toggle_5");
                    return new Result(path, GetResultForToggle(5));
                }
                else
                {
                    path.Add("no_3_3");
                    path.Add("toggle_6");
                    return new Result(path, GetResultForToggle(6));
                }
            }
            else
            {
                path.Add("no_2_2");
                path.Add("3_4");
                if (FindNodeText("3_4") == "?")
                {
                    return new Result(path, false);
                }

                if (AnswerNode("3_4"))
                {
                    path.Add("yes_3_4");
                    path.Add("toggle_7");
                    return new Result(path, GetResultForToggle(7));
                }
                else
                {
                    path.Add("no_3_4");
                    path.Add("toggle_8");
                    return new Result(path, GetResultForToggle(8));
                }
                
            }
        }
    }

    private bool GetResultForToggle(int number)
    {
        foreach (Button button in decisionTree.GetComponentsInChildren<Button>())
        {
            if (button.name == $"toggle_{number}")
            {
                if (button.GetComponent<ToggleButton>().isOn)
                {
                    return true;
                }
                return false;
            }
        }

        return false;
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

        return "";
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
