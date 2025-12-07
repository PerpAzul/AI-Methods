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

    public void Awake() {
        initialAnchoredPosition = this.GetComponent<TextMeshProUGUI>().GetComponent<RectTransform>().anchoredPosition;
    }

    public void TriggerText(string message)
    {
        text = this.GetComponent<TextMeshProUGUI>();
        text.text = message;
        StartCoroutine(FloatAndFade(1));
    }

    private IEnumerator FloatAndFade(int seconds)
    {
        string message = text.text;
        text.text = "";
        yield return new WaitForSeconds(seconds);
        text.text = message;
        // Startzustand
        Color color = text.color;
        color.a = 1;
        text.color = color;

        RectTransform rt = text.GetComponent<RectTransform>();
        Vector3 startPos = initialAnchoredPosition + (Vector2) startOffset;
        Vector3 endPos = initialAnchoredPosition + (Vector2) endOffset;

        rt.anchoredPosition = startPos;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;

            // Bewegung von unten zur Mitte
            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, t);

            // Farbverlauf (Alpha reduzieren)
            float fadeT = Mathf.Clamp01(t * moveDuration / fadeDuration);
            color.a = Mathf.Lerp(1, 0, fadeT);
            text.color = color;

            yield return null;
        }
    }
}

