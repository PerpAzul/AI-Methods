using UnityEngine;
using System.Collections;
using TMPro;

public class CongratsMessage : MonoBehaviour
{
    public TMP_Text[] text;
    public float moveDuration = 2f;
    public float fadeDuration = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (VariableStore.IsGameFinished()) {
            this.gameObject.SetActive(false);
            return;
        }
        StartCoroutine(FadeOut());
        VariableStore.SetGameStateFinished(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < text.Length; i++) {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FloatAndFade(text[i]));
        }
        yield return new WaitForSeconds(10f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator FloatAndFade(TMP_Text text)
    {
        // Startzustand
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        float t = 0f;

        while (t < 1f)
        {
            // IMPORTANT: unscaled time so it also works while the game is paused / timescale 0
            float dt = Time.unscaledDeltaTime;
            t += dt / Mathf.Max(0.0001f, moveDuration);

            // Farbverlauf (Alpha reduzieren)
            float fadeT = Mathf.Clamp01(t * moveDuration / Mathf.Max(0.0001f, fadeDuration));
            color.a = Mathf.Lerp(1f, 0f, fadeT);
            text.color = color;

            yield return null;
        }
    }
}
