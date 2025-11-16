using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public bool isOn;
    public Sprite PositiveSprite;
    public Sprite NegativeSprite;
    public Image targetImage;

    private Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(ToggleState);

        UpdateVisual();
    }

    private void ToggleState()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        targetImage.GetComponent<Image>().sprite = isOn ? PositiveSprite : NegativeSprite;
    }
}
