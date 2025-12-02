using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PointDisplay : MonoBehaviour
{
    [SerializeField]
    public GameObject parentCanvas;
    public static PointDisplay Instance;

    private int score = 0;

    private int lastCorrect = 0;
    private int lastIncorrect = 0;

    private TextMeshProUGUI scoreText;

    private float flashDuration = 0.7f;
    private float flashTimer = 0f;
    private Color baseColor = Color.white;
    private Color targetColor = Color.white;

    // Tracks correct edges that have already given points
    private HashSet<string> scoredCorrectEdges = new HashSet<string>();

    void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameObject textObj = new GameObject("ScoreText");
        textObj.transform.SetParent(parentCanvas.transform);

        scoreText = textObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 30;
        scoreText.alignment = TextAlignmentOptions.TopLeft;
        scoreText.color = baseColor;
        scoreText.text = "Punkte: 0";

        RectTransform rect = scoreText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        rect.sizeDelta = new Vector2(400, 100);
    }


    void Update()
    {
        var lm = LevelManager.Instance;
        if (lm == null) return;

        int currentCorrect = lm.getCorrectEdgesCount();
        int currentIncorrect = lm.getIncorrectEdgesCount();

        // New correct edges
        if (currentCorrect > lastCorrect)
        {
            int diff = currentCorrect - lastCorrect;

            // Check which correct edges are new by scanning playerEdges
            foreach (var edge in lm.playerEdges)
            {
                if (LevelStorage.Instance.containsEdge(edge.Item1, edge.Item2, lm.currentLevel))
                {
                    string key = NormalizeEdge(edge.Item1, edge.Item2);

                    if (!scoredCorrectEdges.Contains(key))
                    {
                        scoredCorrectEdges.Add(key);
                        score += 50;
                        FlashColor(new Color(0.3f, 1f, 0.3f));
                        UpdateScoreText();

                        // spawn floating +50 above the new line
                        if (lm.getLastAddedNode1() != null && lm.getLastAddedNode2() != null)
                        {
                            Vector3 mid = (lm.getLastAddedNode1().position + lm.getLastAddedNode2().position) / 2f;
                            SpawnFloatingText("+50", new Color(0.3f, 1f, 0.3f), mid);
                        }
                    }
                }
            }
        }

        // New incorrect edges
        if (currentIncorrect > lastIncorrect)
        {
            int diff = currentIncorrect - lastIncorrect;
            score -= diff * 30;
            FlashColor(new Color(1f, 0.3f, 0.3f));
            UpdateScoreText();

            // spawn -30 above the new incorrect line
            if (lm.getLastAddedNode1() != null && lm.getLastAddedNode2() != null)
            {
                Vector3 mid = (lm.getLastAddedNode1().position + lm.getLastAddedNode2().position) / 2f;
                SpawnFloatingText("-30", new Color(1f, 0.3f, 0.3f), mid);
            }

        }

        lastCorrect = currentCorrect;
        lastIncorrect = currentIncorrect;

        // Fade color back to white
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            float t = 1f - (flashTimer / flashDuration);
            scoreText.color = Color.Lerp(targetColor, baseColor, t);
        }
    }

    private string NormalizeEdge(string a, string b)
    {
        return (a.CompareTo(b) < 0) ? a + "-" + b : b + "-" + a;
    }

    private void FlashColor(Color color)
    {
        targetColor = color;
        scoreText.color = color;
        flashTimer = flashDuration;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Punkte: " + score;
    }

    private void SpawnFloatingText(string text, Color color, Vector3 worldPosition) {
        GameObject go = new GameObject("FloatingScoreText");
        go.transform.position = worldPosition;

        var floating = go.AddComponent<FloatingScoreText>();
        floating.SetText(text, color);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;    
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Find any canvas
        Canvas newCanvas = FindFirstObjectByType<Canvas>();
        if (newCanvas != null)
        {
            parentCanvas = newCanvas.gameObject;
            if (scoreText == null)
            {
                // Rebuild the text if somehow destroyed
                GameObject textObj = new GameObject("ScoreText");
                textObj.transform.SetParent(parentCanvas.transform, false);
                scoreText = textObj.AddComponent<TextMeshProUGUI>();
                scoreText.alignment = TextAlignmentOptions.TopLeft;
                scoreText.color = baseColor;
            }
            else
            {
                scoreText.transform.SetParent(parentCanvas.transform, false);
            }

            scoreText.fontSize = 30;
            RectTransform rect = scoreText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(20f, -20f);
        }
        UpdateScoreText(); 
    }

    public void ResetForNewLevel()
{
    lastCorrect = 0;
    lastIncorrect = 0;
    scoredCorrectEdges.Clear();
}

    public int GetScore() {
        return score;
    }

}
