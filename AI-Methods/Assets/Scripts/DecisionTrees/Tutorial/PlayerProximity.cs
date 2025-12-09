using UnityEngine;
using TMPro;

public class PlayerProximity : MonoBehaviour
{
    public TextMeshProUGUI interactionText; 
    public Canvas worldSpaceCanvas;  

    private HintDisplayer current;

    private Camera myCamera;
    private bool isActive = false;
    void Start()
    {
        worldSpaceCanvas.enabled = false;
        myCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            current = other.GetComponent<HintDisplayer>();
            isActive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HintDisplayer>() == current)
        {
            isActive = false;
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
            if (current.isEnabled)
            {
                isActive = true;
                interactionText.text = current.message;
                worldSpaceCanvas.enabled = true;
                worldSpaceCanvas.transform.position =
                current.transform.position + current.uiOffset;
            } 

        } 
        else
        {
            current = null;
            isActive = false;
            worldSpaceCanvas.enabled = false;
        }
    }
}