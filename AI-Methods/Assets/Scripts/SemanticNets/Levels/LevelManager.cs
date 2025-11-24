using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int currentLevel = 1;
    private List<(string, string)> playerEdges;
    private int correctEdges = 0;
    private int incorrectEdges = 0;

    // IMPORTANT: when adding a new level change
    // 1: maxLevels
    // 2: getListForLevel()
    // Naming for new level scenes: "SemanticNets" + level
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        playerEdges = new List<(string, string)>();
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
        return LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel);
    }

    public Color GetLineColor(Transform node1, Transform node2) {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();
        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;
        if (LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel)) {
            return Color.green;
        } else {
            return Color.red;
        }
    }
    
    // is called when an edge between two nodes (not between player and node!) is created
    // called in ConnectTo() method of LineManager
    public void addEdge(Transform node1, Transform node2) {
        // check if max. edge amount reached
        if (HasReachedMaxEdges()) {
            return;
        }
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

        checkForLevelCompleteScreen();
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

        checkForLevelCompleteScreen();
    }

    public bool isLevelComplete() {
        if (correctEdges == LevelStorage.Instance.levels[currentLevel - 1].Count && incorrectEdges == 0) {
            Debug.Log("Level " + currentLevel + " complete!");
            return true;
        }
        return false;
    }

    public void checkForLevelCompleteScreen() {
        if (isLevelComplete()) {
            // logic for any global updates comes here
            LevelComplete.ShowLevelCompleteScreen(currentLevel);
        }
    }

    // called when pressing the button in the level complete screen
    public void loadNextLevel() {
        currentLevel++;
        correctEdges = 0;
        incorrectEdges = 0;
        playerEdges.Clear();
        
        if (currentLevel > LevelStorage.Instance.levels.Length) {
            SceneManager.LoadScene("Lobby");
            return;
        }
        SceneManager.LoadScene("SemanticNets" + currentLevel);
    }

    //Called in addEdge
    //maxEdges = amount necessary to complete level. 
    public bool HasReachedMaxEdges() {
    int maxEdges = LevelStorage.Instance.levels[currentLevel - 1].Count;
    int currentEdges = correctEdges + incorrectEdges;
    return currentEdges >= maxEdges;
}

    public int getCorrectEdgesCount() {
        return correctEdges;
    }

    public int getIncorrectEdgesCount() {
        return incorrectEdges;
    }
}
