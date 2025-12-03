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
        Debug.Log(other.name);
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

    void Update()
    {
        if (current)
        {
            interactionText.text = current.message;
            worldSpaceCanvas.enabled = true;
            // Follow the object
            worldSpaceCanvas.transform.position =
                current.transform.position + current.uiOffset;

            // Look at the player camera
            Transform cam = myCamera.transform;
            worldSpaceCanvas.transform.LookAt(worldSpaceCanvas.transform.position + cam.forward);
        }
    }
}