using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MinimapController : MonoBehaviour
{
    public static MinimapController Instance;

    [Header("Minimap UI")]
    public Canvas endingCanvas;
    public Button button;
    public string[] explanationText;
    public TextMeshProUGUI explanationTextField;
    public MinimapAnimator animator;
    private int textIdx = 0;

    [Header("Elements to hide")]
    public GameObject[] elementsToHide;         // Icons, Marker usw. die ausgeblendet werden sollen

    public void Awake() {
        Instance = this;
        if (button != null) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
        }
        explanationTextField.text = explanationText.Length > 0 ? explanationText[0] : "";
        endingCanvas.gameObject.SetActive(false);
    }

    public void ShowLargeMinimap()
    {
        Time.timeScale = 1f;
        StartCoroutine(ShowLargeMinimapRoutine());
    }

    private IEnumerator ShowLargeMinimapRoutine()
    {
        // 1. Minimap groß anzeigen
        endingCanvas.gameObject.SetActive(true);

        // CanvasGroup endingCanvasGroup = endingCanvas.GetComponent<CanvasGroup>();
        // endingCanvasGroup.alpha = 0f;
        // endingCanvasGroup.LeanAlpha(1f, 0.25f);
        yield return new WaitForSeconds(0.5f); // kleiner Buffer Frame

        // 2. Bestimmte UI-Elemente ausblenden
        foreach (var obj in elementsToHide)
            obj.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        // 3. Animation ausführen
        if (animator != null)
        {
            animator.AnimateElements();
        }

        // Falls die Animation Zeit braucht:
        yield return new WaitForSeconds(1f);
    }

    public void OnButtonClick()
    {
        if (textIdx >= explanationText.Length - 1)
        {
            endingCanvas.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            LevelManager.Instance.loadNextLevel();
            return;
        }
        explanationTextField.text = explanationText[++textIdx];
    }
}
