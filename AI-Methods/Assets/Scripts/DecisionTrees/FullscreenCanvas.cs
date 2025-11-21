using Unity.Cinemachine;
using UnityEngine;

public class FullscreenCanvas : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && fullscreenCanvas.gameObject.activeSelf)
        {
            fullscreenCanvas.gameObject.SetActive(false);
            myCamera.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
