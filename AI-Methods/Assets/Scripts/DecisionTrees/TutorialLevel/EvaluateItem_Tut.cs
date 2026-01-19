using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DecisionTrees;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateItem_Tut : MonoBehaviour
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
    [Header("Database")] public Database_Tut database;
    public ProgressBar_Tut progressBar;
    public NPCGuide guide;

    [SerializeField]
    // damit Maschine nur getestet werden kann wenn player im collider der maschine
    private CanvasToggle playerNearCanvasToggle;

    public RectTransform warning;
    private bool isTesting = false;

    private IEnumerator TestDatabase()
    {
        isTesting = true;
        if(guide) guide.ContinueIfCurrentActionEquals("first_test");
        warning.gameObject.SetActive(false);
        float progress = 0;
        foreach (Item_Tut item in database.ScannedItems)
        {
            Result result = Evaluate(item.IsRed, item.IsFruit);
            HighlightPath(result.Path);
            database.DisplayEvaluate(item, progress/database.ScannedItems.Count);
            yield return new WaitForSeconds(0.5f);

            
            if (guide && result.Path[^1] == "toggle_1")
            {
                guide.ContinueIfCurrentActionEquals("second_test");
            }

            if(guide && item.IsFruit && !item.IsRed) guide.ContinueIfCurrentActionEquals("banana_test");
            if(guide && !item.IsFruit && item.IsRed) guide.ContinueIfCurrentActionEquals("watermelon_test");

            if(guide && !item.IsFruit && !item.IsRed) guide.ContinueIfCurrentActionEquals("carrot_test");
            
            if (!result.Path[^1].StartsWith("toggle"))
            {
                DisplayPathWarning(result.Path[^1]);
                database.DisplayResult("?", progress/database.ScannedItems.Count);
                break;
            }
            else if(result.Useful == item.Useful())
            {
                if (item.IsFruit && item.IsRed)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("machine_t_correct");
                }

                if (item.IsFruit && !item.IsRed)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("banana_correct");
                }
                if (!item.IsFruit && item.IsRed)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("watermelon_correct");
                }
                progress++;
                progressBar.curr = progress;

                database.DisplayResult("KORREKT", progress/database.ScannedItems.Count);
                yield return new WaitForSeconds(1);
            }
            else
            {
                if (item.IsFruit && !item.IsRed)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("banana_incorrect");
                }
                if (!item.IsFruit && item.IsRed)
                {
                    if (guide) guide.ContinueIfCurrentActionEquals("watermelon_incorrect");
                }

                database.DisplayResult("FALSCH", progress/database.ScannedItems.Count);
                DisplayResultError(result.Path[^1]);
                break;
            }
        }
        isTesting = false;
    }

    private void Start()
    {
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar_Tut>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && playerNearCanvasToggle.isPlayerNear)
        {
            if (!isTesting)
            {
                StartCoroutine(TestDatabase());
            }
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

    private bool red, fruit;
    private Result Evaluate(bool isRed, bool isFruit)
    {
        List<string> path = new();
        red = isRed;
        fruit = isFruit;
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
                path.Add("toggle_1");
                return new Result(path, GetResultForToggle(1));
            }
            else
            {
                path.Add("no_2_1");
                path.Add("toggle_2");
                return new Result(path, GetResultForToggle(2));
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
                path.Add("toggle_3");
                return new Result(path, GetResultForToggle(3));
            }
            else
            {
                path.Add("no_2_2");
                path.Add("toggle_4");
                return new Result(path, GetResultForToggle(4));
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
        if (text == "Rot?" && red || text == "Pilz?" && fruit)
        {
            return true;
        }

        return false;
    }
}
