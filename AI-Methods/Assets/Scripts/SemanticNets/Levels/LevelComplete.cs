using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public static void ShowLevelCompleteScreen(int level) {
        GameObject canvasObject = new GameObject("LevelCompleteCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        canvasObject.AddComponent<GraphicRaycaster>();

        Image image = canvasObject.AddComponent<Image>();

        Color color;
        ColorUtility.TryParseHtmlString("#fff06b34", out color);
        image.color = color;

        // Main text
        TextMeshProUGUI text = new GameObject("Text (TMP)").AddComponent<TextMeshProUGUI>();
        text.transform.SetParent(canvasObject.transform);
        text.text = "Level " + level + " geschafft!\n";
        text.fontSize = 72;
        text.alignment = TextAlignmentOptions.Center;
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(600, 200);
        rectTransform.anchoredPosition = new Vector2(0, 10);

        // Score text
        TextMeshProUGUI scoreText = new GameObject("ScoreText").AddComponent<TextMeshProUGUI>();
        scoreText.transform.SetParent(canvasObject.transform);
        scoreText.text = "Punkte: " + ScoreSystem.Instance.GetScore();
        scoreText.fontSize = 48;
        scoreText.alignment = TextAlignmentOptions.Center;

        RectTransform scoreRect = scoreText.GetComponent<RectTransform>();
        scoreRect.sizeDelta = new Vector2(600, 100);
        scoreRect.anchoredPosition = new Vector2(0, -60);

        // Button
        GameObject buttonObj = new GameObject("ContinueButton");
        buttonObj.transform.SetParent(canvasObject.transform);

        Image buttonImage = buttonObj.AddComponent<Image>();
        Button button = buttonObj.AddComponent<Button>();
        buttonImage.color = Color.white;

        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchoredPosition = new Vector2(0, -150);

        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Weiter";
        tmp.fontSize = 30;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        button.onClick.AddListener(() =>
        {
            Debug.Log("Button clicked!");
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            LevelManager.Instance.loadNextLevel();
            Destroy(canvasObject);
        });

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
