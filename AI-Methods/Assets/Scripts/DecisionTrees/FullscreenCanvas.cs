using UnityEngine;
using UnityEngine.UI;

public class FullscreenCanvas : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public Button myButton;
    public RawImage image;
    public Text text;

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnMyButtonClick);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && fullscreenCanvas.gameObject.activeSelf)
        {
            myButton.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
            fullscreenCanvas.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void OnMyButtonClick()
    {
        myButton.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
    }
}
