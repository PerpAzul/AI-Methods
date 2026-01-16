using UnityEngine;

public class InteractRoadmap : InteractableI
{
    public GameObject roadmapCanvas;

    void Start() {
        roadmapCanvas.SetActive(false);
    }

    protected override void Interact()
    {
        roadmapCanvas.SetActive(!roadmapCanvas.activeSelf);
        Time.timeScale = roadmapCanvas.activeSelf ? 0f : 1f;
        Cursor.visible = roadmapCanvas.activeSelf ? true : false;
        Cursor.lockState = roadmapCanvas.activeSelf ? CursorLockMode.None: CursorLockMode.Locked;
    }
}
