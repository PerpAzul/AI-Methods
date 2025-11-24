using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public bool isOn;
    public Sprite PositiveSprite;
    public Sprite NegativeSprite;
    public Image targetImage;

    private Button myButton;
    private GameManager gameManager;

    void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(ToggleState);

        UpdateVisual();
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void ToggleState()
    {
        isOn = !isOn;
        gameManager.resetProgress();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        targetImage.GetComponent<Image>().sprite = isOn ? PositiveSprite : NegativeSprite;
    }
}
