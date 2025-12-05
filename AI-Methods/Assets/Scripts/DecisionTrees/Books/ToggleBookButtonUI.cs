using Unity.Cinemachine;
using UnityEngine;

public class ToggleBookButtonUI : MonoBehaviour
{
    public GameObject buttonUI;
    public bool isActive = false;
    [SerializeField] CinemachineCamera cam;
    void Update()
    {
        // make sure you can still interact with ui after pausing
        if (buttonUI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isActive)
        {
            cam.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isActive = false;
            buttonUI.SetActive(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isActive)
        {
            cam.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isActive = true;
            buttonUI.SetActive(true);
            return;
        }

    }
}
