using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MinimapController : MonoBehaviour
{
    public static MinimapController Instance;

    [Header("Minimap UI")]
    public Canvas endingCanvas;
    public Button nextButton;
    public Button backButton;
    public Button lobbyButton;
    public string[] explanationText;
    public TextMeshProUGUI explanationTextField;
    public MinimapAnimator animator;
    private int textIdx = 0;

    [Header("Elements to hide")]
    public GameObject[] elementsToHide;         // Icons, Marker usw. die ausgeblendet werden sollen

    public void Awake() {
        Instance = this;
        if (nextButton != null) {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
        if (backButton != null) {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnBackButtonClick);
        }
        if (lobbyButton != null) {
            lobbyButton.onClick.RemoveAllListeners();
            lobbyButton.onClick.AddListener(OnLobbyButtonClick);
            lobbyButton.gameObject.SetActive(false);
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
        // Minimap groß anzeigen
        endingCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f); // kleiner Buffer Frame

        // Bestimmte UI-Elemente ausblenden
        foreach (var obj in elementsToHide)
            obj.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        // Animation ausführen
        if (animator != null)
        {
            animator.AnimateElements();
        }

        // Falls die Animation Zeit braucht:
        yield return new WaitForSeconds(1f);
    }

    public void OnNextButtonClick()
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
        if (textIdx == explanationText.Length - 1)
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nächstes Level";
            nextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(167, nextButton.GetComponent<RectTransform>().sizeDelta.y);
            if (lobbyButton != null) {
                lobbyButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnBackButtonClick()
    {
        if (textIdx <= 0)
            return;
        explanationTextField.text = explanationText[--textIdx];
        if (lobbyButton != null) {
            lobbyButton.gameObject.SetActive(false);
        }
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Weiter";
        nextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(105.08f, nextButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void OnLobbyButtonClick()
    {
        endingCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LevelManager.Instance.loadLobby();
    }
}
