using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    [Tooltip("1 for SemanticNets1, 2 for SemanticNets2, etc.")]
    public int currentLevel = 1;

    [Tooltip("Scene to load when this level is completed")]
    [SerializeField] private string nextSceneName = "SemanticNets2";

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private FloatingExplanationText floatingExplanationText;

    [Header("UI – Level Complete")]
    [SerializeField] private GameObject levelCompletePanel; // LevelCompleteCanvas root
    [SerializeField] private TMP_Text titleText;            // big 'Level X geschafft!'
    [SerializeField] private TMP_Text scoreText;            // 'Punkte: XXX'
    [SerializeField] private Button continueButton;

    [Header("UI – Gameplay to hide on win (optional)")]
    [SerializeField] private GameObject minimapCanvas;      // Jammo minimap canvas, or null

    // gameplay state
    public List<(string, string)> playerEdges = new();

    private int correctEdges = 0;
    private int incorrectEdges = 0;

    private Transform lastAddedNode1;
    private Transform lastAddedNode2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // make sure panel starts hidden
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueButton);
        }
        else
        {
            Debug.LogWarning("LevelManager: ContinueButton is not assigned on " + gameObject.name);
        }
    }

    // Validation
    public bool isValidConnection(Transform node1, Transform node2)
    {
        if (node1 == null || node2 == null)
            return false;

        if (node1.Equals(node2))
            return false;

        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd   = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = Normalize(tmpStart.text);
        string node2Text = Normalize(tmpEnd.text);


        foreach (var edge in playerEdges)
        {
            if ((edge.Item1 == node1Text && edge.Item2 == node2Text) ||
                (edge.Item1 == node2Text && edge.Item2 == node1Text))
            {
                return false; // already exists
            }
        }

        return true;
    }

    public bool isCorrectConnection(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd   = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        return LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel);
    }

    public Color GetLineColor(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd   = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = tmpStart.text;
        string node2Text = tmpEnd.text;

        return LevelStorage.Instance.containsEdge(node1Text, node2Text, currentLevel)
            ? Color.green
            : Color.red;
    }

    // Edge management
    public void addEdge(Transform node1, Transform node2)
    {
        if (HasReachedMaxEdges())
            return;

        if (!isValidConnection(node1, node2))
            return;

        if (isCorrectConnection(node1, node2)) {
            correctEdges++;
        } else {
            incorrectEdges++;
        }

        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd   = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = Normalize(tmpStart.text);
        string node2Text = Normalize(tmpEnd.text);

        playerEdges.Add((node1Text, node2Text));

        var pair = LevelStorage.Instance.getPair(node1Text, node2Text, currentLevel);
        if (pair.HasValue)
        {
            if (floatingExplanationText != null)
                floatingExplanationText.TriggerText($"<size=70%>Logische Verbindung:</size>\n{pair.Value.Item2} ist {pair.Value.Item1}");
        }

        lastAddedNode1 = node1;
        lastAddedNode2 = node2;

        checkForLevelCompleteScreen();
    }

    public void removeEdge(Transform node1, Transform node2)
    {
        TMP_Text tmpStart = node1.GetChild(0).GetComponent<TextMeshPro>();
        TMP_Text tmpEnd   = node2.GetChild(0).GetComponent<TextMeshPro>();

        string node1Text = Normalize(tmpStart.text);
        string node2Text = Normalize(tmpEnd.text);


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

    // Level completion
    public void checkForLevelCompleteScreen()
    {
        bool allCorrect  = correctEdges == LevelStorage.Instance.levels[currentLevel].Count;
        bool noIncorrect = incorrectEdges == 0;

        if (!allCorrect || !noIncorrect)
            return;

        // hide minimap 
        if (minimapCanvas != null)
            minimapCanvas.SetActive(false);

        // title text
        if (titleText != null)
            titleText.text = "Level " + currentLevel + " geschafft!";

        // score text
        if (scoreText != null && PointDisplay.Instance != null)
        {
            int currentScore = PointDisplay.Instance.GetScore();
            scoreText.text = "Punkte: " + (currentScore + PointDisplay.Instance.GetAdd());
        } else {
            scoreText.text = "";
        }

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnContinueButton()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        loadNextLevel();
    }


    // Level transitions
    public void loadNextLevel()
    {
        correctEdges = 0;
        incorrectEdges = 0;
        playerEdges.Clear();

        // reset per-level tracking in PointDisplay, but keep total score if you want
        if (PointDisplay.Instance != null)
            PointDisplay.Instance.ResetForNewLevel();

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(LoadSceneRoutine(nextSceneName));
        }
        else
        {
            // fallback – go back to lobby or something
            StartCoroutine(LoadSceneRoutine("Lobby German"));
        }
    }

    // Edge limit
    public bool HasReachedMaxEdges()
    {
        if (currentLevel > LevelStorage.Instance.levels.Length)
            return false;

        int maxEdges     = LevelStorage.Instance.levels[currentLevel].Count;
        int currentEdges = correctEdges + incorrectEdges;
        return currentEdges >= maxEdges;
    }

    private System.Collections.IEnumerator LoadSceneRoutine(string nextSceneName)
    {
        loadingScreen.SetActive(true);
        yield return null;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    //Normalize Text to recognize Strings that aren't exactly equal
        private string Normalize(string s)
    {
        return s.Normalize(System.Text.NormalizationForm.FormC).Trim();
    }


    // Getters
    public int getCorrectEdgesCount() => correctEdges;
    public int getIncorrectEdgesCount() => incorrectEdges;
    public int getMaxEdgesCount() => LevelStorage.Instance.levels[currentLevel].Count;
    public Transform getLastAddedNode1() => lastAddedNode1;
    public Transform getLastAddedNode2() => lastAddedNode2;
}
