using TMPro;
using UnityEngine;

public static class SceneFontReference {
    public static TMP_FontAsset Font;
}

public class SceneFonts : MonoBehaviour
{
    [Header("Assign your TMP Font Asset here")]
    public TMP_FontAsset fontAsset;

    void Start()
    {
        if (fontAsset == null)
        {
            Debug.LogWarning("⚠ No font assigned on SceneFontSetter");
            return;
        }

        ApplyFontToScene();
    }

    void ApplyFontToScene()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);

        foreach (var t in allTexts)
        {
            t.font = fontAsset;
        }

        Debug.Log($"▶ Applied font to {allTexts.Length} TMP objects");
    }
}
