using UnityEngine;
using TMPro;

public class PlayerProximity : MonoBehaviour
{
    public TextMeshProUGUI interactionText; 
    public Canvas worldSpaceCanvas;  

    private HintDisplayer current;

    private Camera myCamera;
    void Start()
    {
        worldSpaceCanvas.enabled = false;
        myCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        current = other.GetComponent<HintDisplayer>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HintDisplayer>() == current)
        {
            current = null;
            worldSpaceCanvas.enabled = false;
        }
    }

    public void Hide()
    {
        current = null;
    }

    void Update()
    {
        if (current)
        {
            interactionText.text = current.message;
            worldSpaceCanvas.enabled = true;
            worldSpaceCanvas.transform.position =
                current.transform.position + current.uiOffset;

        }
    }
}