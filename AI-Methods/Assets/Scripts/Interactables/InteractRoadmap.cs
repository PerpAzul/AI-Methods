using UnityEngine;

public class InteractRoadmap : InteractableI
{
    public Canvas roadmapCanvas;

    void Start() {
        roadmapCanvas.enabled = false;
    }

    protected override void Interact()
    {
        Time.timeScale = roadmapCanvas.enabled ? 1f : 0f;
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = roadmapCanvas.enabled ? CursorLockMode.Locked : CursorLockMode.None;
        roadmapCanvas.enabled = !roadmapCanvas.enabled;
    }
}
