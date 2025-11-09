using UnityEngine;

public class InstructionBook : Interactable
{
    [SerializeField]
    public GameObject panel;        // Dein UI-Panel
    
    public static InstructionBook Instance;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    protected override void Interact()
    {
        panel.SetActive(true);
    }

    protected override void Destroy()
    {
        panel.SetActive(false);
    }
}
