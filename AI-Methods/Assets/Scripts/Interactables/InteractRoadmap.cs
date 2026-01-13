using UnityEngine;

public class InteractRoadmap : InteractableI
{
    public Canvas roadmapCanvas;

    void Start() {
        roadmapCanvas.enabled = false;
    }

    protected override void Interact()
    {
        roadmapCanvas.enabled = !roadmapCanvas.enabled;
        Time.timeScale = roadmapCanvas.enabled ? 0f : 1f;
        Cursor.visible = roadmapCanvas.enabled ? true : false;
        Cursor.lockState = roadmapCanvas.enabled ? CursorLockMode.None: CursorLockMode.Locked;
    }
}
