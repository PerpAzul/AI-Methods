using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private Button backButton; // Back button from Options menu
    
    private InputAction cancelAction;

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }
        
        // Bind the "Cancel" action manually (Escape / B on gamepad)
        cancelAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        cancelAction.AddBinding("<Gamepad>/buttonEast"); // optional (B on Xbox, Circle on PlayStation)
        cancelAction.performed += _ => OnCancel();
        cancelAction.Enable();
    }

    void OnDisable()
    {
        cancelAction.performed -= _ => OnCancel();
        cancelAction.Disable();
    }
    
    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (optionsUI.activeSelf)
        {
            optionsUI.SetActive(false);
            mainUI.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenOptionsMenu()
    {
        mainUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        optionsUI.SetActive(false);
        mainUI.SetActive(true);
    }
    
    private void OnCancel()
    {
        // Only react if Options menu is currently open
        if (optionsUI.activeSelf)
            BackToPauseMenu();
    }
}
