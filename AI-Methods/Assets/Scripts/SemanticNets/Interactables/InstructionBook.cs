using UnityEngine;

public class InstructionBook : InteractableI
{
    [SerializeField]
    public GameObject panel;        // Dein UI-Panel
    private bool active = false;
    
    public static InstructionBook Instance;

    void Awake()
    {
        Instance = this;
        panel.SetActive(active);
    }

    protected override void Interact()
    {
        active = !active;
        panel.SetActive(active);
    }
}
