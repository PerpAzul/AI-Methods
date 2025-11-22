using UnityEngine;
using Unity.Cinemachine;

public class BookToggle : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;

    private bool isNear;
    void OnTriggerEnter(Collider other)
    {
        isNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        isNear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            fullscreenCanvas.gameObject.SetActive(true);
            myCamera.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(gameObject);
        }
    }
}
