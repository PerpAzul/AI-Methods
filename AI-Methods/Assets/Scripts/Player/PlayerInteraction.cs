using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private PlayerInput playerInput;
    private InputAction interactAction;
    private ThirdPersonUI thirdUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();
        thirdUI = GetComponent<ThirdPersonUI>();
    }

    // Update is called once per frame
    void Update()
    {
        thirdUI.UpdateText(string.Empty);
        Collider[] hits = Physics.OverlapBox(transform.position + transform.forward * 0.25f, transform.localScale*0.5f, transform.rotation, mask, QueryTriggerInteraction.Collide);
        foreach (Collider hit in hits)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
            {
                thirdUI.UpdateText(interactable.promptMessage);
                break; // stop after first valid hit
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
        Collider[] hits = Physics.OverlapBox(transform.position + transform.forward * 0.25f, transform.localScale*0.5f, transform.rotation, mask, QueryTriggerInteraction.Collide);
        foreach (Collider hit in hits)
        {
            InteractableI interactable = hit.GetComponent<InteractableI>();
            if (interactable != null)
            {
                interactable.BaseInteract();
                break; // stop after first valid hit
            }
        }
    }
}
