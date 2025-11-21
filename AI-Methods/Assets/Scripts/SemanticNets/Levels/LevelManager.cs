using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int currentLevel = 1;
    private List<(string, string)> playerEdges;
    private int correctEdges = 0;
    private int incorrectEdges = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        playerEdges = new List<(string, string)>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks valid connection in terms of if it is conceptually allowed to draw this connection
    public bool isValidConnection(Transform node1, Transform node2) {
        if (node1.Equals(node2)) {
            return false; // gleiche Knoten können nicht verbunden werden
        }
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        foreach (var edge in playerEdges) {
            if ((edge.Item1 == node1Text && edge.Item2 == node2Text) || (edge.Item1 == node2Text && edge.Item2 == node1Text)) {
                return false; // Verbindung existiert bereits
            }
        }
        return true;
    }

    // Checks if the connection is correct according to the level's rules
    public bool isCorrectConnection(Transform node1, Transform node2) {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        if (currentLevel == 1) {
            return Level1Storage.Instance.containsEdge(node1Text, node2Text);
        }
        // more levels can be added here
        return false;
    }

    public Color GetLineColor(int level, Transform node1, Transform node2) {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        if (level == 1) {
            if (Level1Storage.Instance.containsEdge(node1Text, node2Text)) {
                return Color.green;
            } else {
                return Color.red;
            }
        // more levels can be added here
        } else {
            return Color.cyan;
        }
    }
    
    // is called when an edge between two nodes (not between player and node!) is created
    // called in ConnectTo() method of LineManager
    public void addEdge(Transform node1, Transform node2) {
        // check connection
        if (!isValidConnection(node1, node2))
            return;
        if (isCorrectConnection(node1, node2)) {
            correctEdges++;
        } else {
            incorrectEdges++;
        }
        // add connection
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        playerEdges.Add((node1Text, node2Text));

        isLevelComplete();
    }

    // is called before an edge between two nodes is "destroyed"
    // called in Interact() method of LineObject
    public void removeEdge(Transform node1, Transform node2) {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        for (int i = 0; i < playerEdges.Count; i++) {
            var edge = playerEdges[i];
            if ((edge.Item1 == node1Text && edge.Item2 == node2Text) || (edge.Item1 == node2Text && edge.Item2 == node1Text)) {
                playerEdges.RemoveAt(i);
                if (isCorrectConnection(node1, node2)) {
                    correctEdges--;
                } else {
                    incorrectEdges--;
                }
                break;
            }
        }

        isLevelComplete();
    }

    public bool isLevelComplete() {
        if (currentLevel == 1) {
            if (correctEdges == Level1Storage.Instance.correctEdges.Count && incorrectEdges == 0) {
                Debug.Log("Level 1 complete!");
                return true;
            }
            return false;
        }
        // more levels can be added here
        return false;
    }

    public int getCorrectEdgesCount() {
        return correctEdges;
    }

    public int getIncorrectEdgesCount() {
        return incorrectEdges;
    }
}
