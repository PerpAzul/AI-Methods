using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public GameObject panel;
    [SerializeField]
    public GameObject canvasToDisable;

    public static LevelManager Instance;
    public int currentLevel = 1;
    public List<(string, string)> playerEdges;
    private int correctEdges = 0;
    private int incorrectEdges = 0;
    private Transform lastAddedNode1; //for FloatScore Text
    private Transform lastAddedNode2; //for FloatScore Text

    // IMPORTANT: when adding a new level change LevelStorage.cs
    // Naming for new level scenes: "SemanticNets" + level (otherwise it won't work!)
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        playerEdges = new List<(string, string)>();
        panel.SetActive(false);
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

        // store last added positions
        lastAddedNode1 = node1;
        lastAddedNode2 = node2;

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

    public void checkForLevelCompleteScreen() {
        if (correctEdges == LevelStorage.Instance.levels[currentLevel - 1].Count && incorrectEdges == 0) {
            // logic for any global updates comes here

            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + currentLevel + " geschafft!";
            panel.SetActive(true);
            canvasToDisable.SetActive(false);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Button btn = panel.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                loadNextLevel();
                panel.SetActive(false);
                canvasToDisable.SetActive(true);
            });
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
        if (currentLevel > LevelStorage.Instance.levels.Length) {
            return false;
        }
        int maxEdges = LevelStorage.Instance.levels[currentLevel - 1].Count;
        int currentEdges = correctEdges + incorrectEdges;
        return currentEdges >= maxEdges;
    }


    // Getters

    public int getCorrectEdgesCount() {
        return correctEdges;
    }

    public int getIncorrectEdgesCount() {
        return incorrectEdges;
    }
    
    public Transform getLastAddedNode1() {
        return lastAddedNode1;
    }

    public Transform getLastAddedNode2() {
        return lastAddedNode2;
    }
}
