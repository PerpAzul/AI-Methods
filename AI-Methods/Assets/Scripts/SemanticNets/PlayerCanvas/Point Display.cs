using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PointDisplay : MonoBehaviour
{
    [SerializeField]
    public GameObject parentCanvas;

    public static PointDisplay Instance;

    // Score should persist across scenes
    private static int score = 0;

    private int lastCorrect = 0;
    private int lastIncorrect = 0;

    private TextMeshProUGUI scoreText;

    private float flashDuration = 0.7f;
    private float flashTimer = 0f;
    private Color baseColor = Color.white;
    private Color targetColor = Color.white;

    // Tracks correct edges that have already given points
    private HashSet<string> scoredCorrectEdges = new HashSet<string>();

    private int add = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // If not set in Inspector, grab first Canvas in the scene
        if (parentCanvas == null)
        {
            Canvas c = FindFirstObjectByType<Canvas>();
            if (c != null)
                parentCanvas = c.gameObject;
        }

        // Create the score text under this scene's canvas
        GameObject textObj = new GameObject("ScoreText");
        textObj.transform.SetParent(parentCanvas.transform, false);

        scoreText = textObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 50;
        scoreText.alignment = TextAlignmentOptions.TopLeft;
        scoreText.color = baseColor;

        RectTransform rect = scoreText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot     = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        rect.sizeDelta = new Vector2(400, 100);

        UpdateScoreText();
    }

    void Update()
    {
        var lm = LevelManager.Instance;
        if (lm == null) return;

        int currentCorrect   = lm.getCorrectEdgesCount();
        int currentIncorrect = lm.getIncorrectEdgesCount();

        // we need level edge count for scoring
        int level = lm.currentLevel;
        int edgeCount = LevelStorage.Instance.levels[level].Count;

        // ---- correct scoring values ----
        float correctP   = 150f / edgeCount;      
        // ---- incorrect scoring values ----
        float incorrectP = (-150f / edgeCount) + 5f;

        // ----------------- correct edges -----------------
        if (currentCorrect > lastCorrect)
        {
            foreach (var edge in lm.playerEdges)
            {
                if (LevelStorage.Instance.containsEdge(edge.Item1, edge.Item2, lm.currentLevel))
                {
                    string key = NormalizeEdge(edge.Item1, edge.Item2);

                    if (!scoredCorrectEdges.Contains(key))
                    {
                        scoredCorrectEdges.Add(key);

                        add = Mathf.RoundToInt(correctP);
                        score += add;
                        Debug.Log("Updated score: " + score);

                        FlashColor(new Color(0.3f, 1f, 0.3f));
                        UpdateScoreText();

                        if (lm.getLastAddedNode1() != null && lm.getLastAddedNode2() != null)
                        {
                            Vector3 mid = (lm.getLastAddedNode1().position + lm.getLastAddedNode2().position) / 2f;
                            SpawnFloatingText("+" + add, new Color(0.3f, 1f, 0.3f), mid);
                        }
                    }
                }
            }
        }

        // ----------------- incorrect edges -----------------
        if (currentIncorrect > lastIncorrect)
        {
            int add = Mathf.RoundToInt(incorrectP);
            score += add;

            FlashColor(new Color(1f, 0.3f, 0.3f));
            UpdateScoreText();

            if (lm.getLastAddedNode1() != null && lm.getLastAddedNode2() != null)
            {
                Vector3 mid = (lm.getLastAddedNode1().position + lm.getLastAddedNode2().position) / 2f;
                SpawnFloatingText(add.ToString(), new Color(1f, 0.3f, 0.3f), mid);
            }
        }

        lastCorrect   = currentCorrect;
        lastIncorrect = currentIncorrect;

        // fade score color back
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

    private void SpawnFloatingText(string text, Color color, Vector3 worldPosition)
    {
        GameObject go = new GameObject("FloatingScoreText");
        go.transform.position = worldPosition;

        var floating = go.AddComponent<FloatingScoreText>();
        floating.SetText(text, color);
    }

    // Called by LevelManager when starting a new level
    public void ResetForNewLevel()
    {
        lastCorrect = 0;
        lastIncorrect = 0;
        scoredCorrectEdges.Clear();
    }

    public int GetScore()
    {
        return score;
    }

    // important for obtaining what was added because score gets updated later than the actual level complete canvas in levelmanager
    public int GetAdd()
    {
        return add;
    }
}
