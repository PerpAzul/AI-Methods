using Unity.Cinemachine;
using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;

    private bool isPlayerNear;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            fullscreenCanvas.gameObject.SetActive(true);
            myCamera.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
