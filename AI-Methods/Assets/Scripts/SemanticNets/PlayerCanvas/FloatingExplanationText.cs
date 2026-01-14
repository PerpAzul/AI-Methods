using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingExplanationText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public float moveDuration = 4f;
    public float fadeDuration = 4f;
    public Vector3 startOffset = new Vector3(0, -200, 0);
    public Vector3 endOffset = new Vector3(0, 0, 0);

    private Vector2 initialAnchoredPosition;

    public void Awake()
    {
        // cache once
        text = GetComponent<TextMeshProUGUI>();
        initialAnchoredPosition = text.GetComponent<RectTransform>().anchoredPosition;
    }

    public void TriggerText(string message)
    {
        // update message immediately (no blank + wait)
        text.text = message;

        // prevent multiple overlapping coroutines
        StopAllCoroutines();
        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        // Startzustand
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        RectTransform rt = text.GetComponent<RectTransform>();
        Vector3 startPos = initialAnchoredPosition + (Vector2)startOffset;
        Vector3 endPos = initialAnchoredPosition + (Vector2)endOffset;

        rt.anchoredPosition = startPos;

        float t = 0f;

        while (t < 1f)
        {
            // IMPORTANT: unscaled time so it also works while the game is paused / timescale 0
            float dt = Time.unscaledDeltaTime;
            t += dt / Mathf.Max(0.0001f, moveDuration);

            // Bewegung von unten zur Mitte
            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, t);

            // Farbverlauf (Alpha reduzieren)
            float fadeT = Mathf.Clamp01(t * moveDuration / Mathf.Max(0.0001f, fadeDuration));
            color.a = Mathf.Lerp(1f, 0f, fadeT);
            text.color = color;

            yield return null;
        }
    }
}
