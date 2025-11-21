using UnityEngine;

public class InstructionBook : InteractableI
{
    [SerializeField]
    public GameObject panel;
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
        if (active) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
        panel.SetActive(active);
    }
}
