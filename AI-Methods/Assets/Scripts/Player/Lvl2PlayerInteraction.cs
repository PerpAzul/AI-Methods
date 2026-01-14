using UnityEngine;
using UnityEngine.InputSystem;

public class Lvl2PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private float interactDistance = 0.5f;

    private PlayerInput playerInput;
    private InputAction interactAction;
    private ThirdPersonUI thirdUI;

    // prevents press+release or double-events in same frame
    private int lastInteractFrame = -1;

    // feedback spam limiter
    private float nextFeedbackTime = 0f;
    [SerializeField] private float feedbackCooldown = 0.6f;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();

        thirdUI = GetComponent<ThirdPersonUI>();
    }

    void OnEnable() => interactAction.Enable();
    void OnDisable() => interactAction.Disable();

    void Update()
    {
        // If menu open: don’t show prompts behind it and don’t accept hotkeys here
        LineTypeMenu menu = FindObjectOfType<LineTypeMenu>(true);
        if (menu != null && menu.IsOpen)
        {
            thirdUI.UpdateText(string.Empty);
            return;
        }

        // Hotkeys 1/2/3
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) HotkeyInteract(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) HotkeyInteract(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) HotkeyInteract(2);

        // Prompt text
        thirdUI.UpdateText(string.Empty);
        Collider hit = FindFirstInteractableHit();
        if (hit != null)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
                thirdUI.UpdateText(interactable.promptMessage);
        }
    }

    // E key (Input System action)
    public void OnInteract(InputValue value)
    {
        // Debounce same-frame duplicates
        if (Time.frameCount == lastInteractFrame)
            return;

        // Ignore release call
        bool pressed;
        try { pressed = value.isPressed; }
        catch { pressed = value.Get<float>() > 0.5f; }

        if (!pressed)
            return;

        lastInteractFrame = Time.frameCount;

        // If menu is open, do nothing (menu closes with R)
        LineTypeMenu menu = FindObjectOfType<LineTypeMenu>(true);
        if (menu != null && menu.IsOpen)
            return;

        if (LineManager.Instance == null)
            return;

        Collider hit = FindFirstInteractableHit();

        // If a line is attached and there's NO target -> cancel/discard
        if (LineManager.Instance.onPlayer() && hit == null)
        {
            LineManager.Instance.CancelCurrentLine();
            ShowFloatingFeedback("Verbindung abgebrochen.");
            return;
        }

        // If a line is attached and we DO have a target -> finish with locked type
        // (E should attach no matter what type -> we use the pending locked type)
        if (LineManager.Instance.onPlayer() && hit != null)
        {
            int lockedIdx = LineManager.Instance.GetPendingIdx();
            LineManager.Instance.ConnectTo(hit.transform, transform, lockedIdx);
            return;
        }

        // Otherwise: normal interaction (ArtObjectLvl2 will open menu on first click)
        if (hit != null)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
                interactable.BaseInteract();
        }
    }

    private void HotkeyInteract(int idx)
    {
        if (LineManager.Instance == null)
            return;

        Collider hit = FindFirstInteractableHit();

        // No interactable in range:
        // - if line attached AND pressed the LOCKED key -> cancel
        // - if line not attached -> do nothing
        if (hit == null)
        {
            if (LineManager.Instance.onPlayer())
            {
                int locked = LineManager.Instance.GetPendingIdx();

                // only cancel if they press the same key as the one that started it
                if (locked == idx)
                {
                    LineManager.Instance.CancelCurrentLine();
                    ShowFloatingFeedback("Verbindung abgebrochen.");
                }
                else
                {
                    ShowFloatingFeedback("Nutze denselben Verbindungstyp wie zuvor.");
                }
            }
            return;
        }

        // If line attached: only allow SAME KEY as the first click
        if (LineManager.Instance.onPlayer())
        {
            int locked = LineManager.Instance.GetPendingIdx();
            if (locked != idx)
            {
                // FIX: show feedback when wrong key is pressed
                ShowFloatingFeedback("Ungültige Eingabe: \n Nutze denselben Verbindungstyp wie zuvor.");
                return;
            }

            LineManager.Instance.ConnectTo(hit.transform, transform, idx);
            return;
        }

        // Start a new line with this type
        LineManager.Instance.ConnectTo(hit.transform, transform, idx);
    }

    private void ShowFloatingFeedback(string msg)
    {
        if (Time.unscaledTime < nextFeedbackTime)
            return;

        nextFeedbackTime = Time.unscaledTime + feedbackCooldown;

        FloatingExplanationText floating = FindObjectOfType<FloatingExplanationText>(true);
        if (floating != null)
            floating.TriggerText(msg);
    }

    private Collider FindFirstInteractableHit()
    {
        Collider[] hits = Physics.OverlapBox(
            transform.position + transform.forward * 0.25f,
            transform.localScale * interactDistance,
            transform.rotation,
            mask,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<InteractableI>() != null)
                return hit;
        }
        return null;
    }
}
