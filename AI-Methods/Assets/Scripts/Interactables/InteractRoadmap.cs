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
    }
}
