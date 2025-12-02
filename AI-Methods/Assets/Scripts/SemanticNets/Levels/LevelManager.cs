using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public GameObject panel;
    public static LevelManager Instance;

    public int currentLevel = 1;
    public List<(string, string)> playerEdges;

    private int correctEdges = 0;
    private int incorrectEdges = 0;

    private Transform lastAddedNode1; // for FloatScore text
    private Transform lastAddedNode2; // for FloatScore text

    private TMP_Text titleText;   // "Level X geschafft!"
    private TMP_Text scoreText;
    // IMPORTANT: When adding a new level, change LevelStorage.cs
    // Naming for new level scenes: "SemanticNets" + level

    // Called when object is created
void Awake() {
    if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    playerEdges = new List<(string, string)>();

    SceneManager.sceneLoaded += OnSceneLoaded;

    // Make sure the very first scene also has the panel set up
    EnsurePanel();
}

void Start()
{
    EnsurePanel();
}

    // --- VALIDATION METHODS ----------------------------------------------------

    // Checks if connection is valid (not same node, not duplicate)
    public bool isValidConnection(Transform node1, Transform node2)
    {
        if (node1.Equals(node2))
            return false;

        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        foreach (var edge in playerEdges)
        {
            if ((edge.Item1 == node1Text && edge.Item2 == node2Text) ||
                (edge.Item1 == node2Text && edge.Item2 == node1Text))
            {
                return false;
            }
        }

        return true;
    }

    // Checks if connection is correct for this level
    public bool isCorrectConnection(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        return LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel);
    }

    // Color for drawn lines
    public Color GetLineColor(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        return LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel)
            ? Color.green
            : Color.red;
    }

    // --- EDGE MANAGEMENT -------------------------------------------------------

    // Called when a new edge between two nodes is created
    public void addEdge(Transform node1, Transform node2)
    {
        if (HasReachedMaxEdges())
            return;

        if (!isValidConnection(node1, node2))
            return;

        if (isCorrectConnection(node1, node2))
            correctEdges++;
        else
            incorrectEdges++;

        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        playerEdges.Add((node1Text, node2Text));

        // Save last added nodes
        lastAddedNode1 = node1;
        lastAddedNode2 = node2;

        checkForLevelCompleteScreen();
    }

    // Called before removing an edge
    public void removeEdge(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        for (int i = 0; i < playerEdges.Count; i++)
        {
            var edge = playerEdges[i];

            if ((edge.Item1 == node1Text && edge.Item2 == node2Text) ||
                (edge.Item1 == node2Text && edge.Item2 == node1Text))
            {
                playerEdges.RemoveAt(i);

                if (isCorrectConnection(node1, node2))
                    correctEdges--;
                else
                    incorrectEdges--;

                break;
            }
        }

        checkForLevelCompleteScreen();
    }

    // --- LEVEL COMPLETION ------------------------------------------------------

public void checkForLevelCompleteScreen()
{
    bool allCorrect  = correctEdges == LevelStorage.Instance.levels[currentLevel - 1].Count;
    bool noIncorrect = incorrectEdges == 0;

    if (allCorrect && noIncorrect)
    {
        if (!EnsurePanel())
            return; // error already logged

        // Set the big title text
        if (titleText != null)
        {
            titleText.text = "Level " + currentLevel + " geschafft!";
        }

        // Set the score text from your point system
        if (scoreText != null)
        {
            int currentScore = 0;

            // TODO: use whatever your point-display script exposes.
            // Example if you have PointDisplay.Instance.GetCurrentScore():
            if (PointDisplay.Instance != null)
                currentScore = PointDisplay.Instance.GetScore();

            scoreText.text = "Punkte: " + (currentScore+50);
        }

        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}



    // Load next level after pressing continue button
    public void loadNextLevel()
    {
        currentLevel++;
        correctEdges = 0;
        incorrectEdges = 0;

        playerEdges.Clear();
        PointDisplay.Instance.ResetForNewLevel();

        if (currentLevel == 1)
            SceneManager.LoadScene("SemanticNets1");
        else if (currentLevel == 2)
            SceneManager.LoadScene("SemanticNets2");
        else
            SceneManager.LoadScene("Lobby");
    }

    // Max edges = required amount to complete level
    public bool HasReachedMaxEdges()
    {
        if (currentLevel > LevelStorage.Instance.levels.Length)
            return false;

        int maxEdges = LevelStorage.Instance.levels[currentLevel - 1].Count;
        int currentEdges = correctEdges + incorrectEdges;

        return currentEdges >= maxEdges;
    }

    // --- UNITY EVENTS ---------------------------------------------------------

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    EnsurePanel();
    Time.timeScale = 1f;
}


private void OnContinueButton()
{
    Time.timeScale = 1f;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    if (panel != null)
        panel.SetActive(false);

    loadNextLevel();
}


private bool EnsurePanel()
{
    // If we already have a valid panel, we’re done
    if (panel != null)
        return true;

    // Try to find it in the current scene
    panel = GameObject.Find("LevelCompleteCanvas");

    if (panel == null)
    {
        Debug.LogError("LevelManager: Could NOT find LevelCompleteCanvas in scene " 
                       + SceneManager.GetActiveScene().name);
        return false;
    }

    // Make sure it starts hidden
    panel.SetActive(false);

    // Wire up the Continue button
    Button btn = panel.GetComponentInChildren<Button>(true);
    if (btn == null)
    {
        Debug.LogError("LevelManager: No Button found under LevelCompleteCanvas!");
    }
    else
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnContinueButton);
    }

    // find the title & score texts under this panel 

    titleText = null;
    scoreText = null;

    TMP_Text[] texts = panel.GetComponentsInChildren<TMP_Text>(true);
    foreach (var t in texts)
    {
        if (t.gameObject.name == "Text (TMP)")
            titleText = t;
        else if (t.gameObject.name == "ScoreText")
            scoreText = t;
    }

    if (titleText == null)
        Debug.LogError("LevelManager: Could not find title TMP_Text under LevelCompleteCanvas.");

    if (scoreText == null)
        Debug.LogError("LevelManager: Could not find ScoreText TMP_Text under LevelCompleteCanvas.");

    return true;
}



    // --- GETTERS 

    public int getCorrectEdgesCount() => correctEdges;
    public int getIncorrectEdgesCount() => incorrectEdges;

    public Transform getLastAddedNode1() => lastAddedNode1;
    public Transform getLastAddedNode2() => lastAddedNode2;
}
