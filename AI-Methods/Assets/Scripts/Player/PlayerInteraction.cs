using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private PlayerInput playerInput;
    private InputAction interactAction;
    private ThirdPersonUI thirdUI;
    [SerializeField] private float interactDistance = 0.5f;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();
        thirdUI = GetComponent<ThirdPersonUI>();
    }

    void Update()
    {
        // If the line-type menu is open, don't show interaction prompts behind it
        LineTypeMenu menu = FindObjectOfType<LineTypeMenu>(true);
        if (menu != null && menu.IsOpen)
        {
            thirdUI.UpdateText(string.Empty);
            return;
        }

        thirdUI.UpdateText(string.Empty);
        Collider[] hits = Physics.OverlapBox(
            transform.position + transform.forward * 0.25f,
            transform.localScale * interactDistance,
            transform.rotation,
            mask,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider hit in hits)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
            {
                thirdUI.UpdateText(interactable.promptMessage);
                break;
            }
        }
    }

    void OnEnable()
    {
        interactAction.Enable();
    }

    void OnDisable()
    {
        interactAction.Disable();
    }

    public void OnInteract(InputValue value)
    {
        // IMPORTANT: Ignore the release call (Input System can fire on press + release)
        bool pressed = false;
        try
        {
            pressed = value.isPressed;
        }
        catch
        {
            pressed = value.Get<float>() > 0.5f;
        }

        if (!pressed)
            return;

        // If menu is open, do nothing (menu handles closing with R)
        LineTypeMenu menu = FindObjectOfType<LineTypeMenu>(true);
        if (menu != null && menu.IsOpen)
            return;

        Collider[] hits = Physics.OverlapBox(
            transform.position + transform.forward * 0.25f,
            transform.localScale * interactDistance,
            transform.rotation,
            mask,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider hit in hits)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
            {
                interactable.BaseInteract();
                break;
            }
        }
    }
}
