using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private Button backButton; // Back button from Options menu
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string gameplayActionMap = "Player";
    [SerializeField] private string uiActionMap = "UI";

    private InputAction pauseAction;
    private InputAction cancelAction;
    private bool isPaused;

    void Start()
    {
        if (playerInput == null)
        {
            return;
        }

        pauseAction = playerInput.actions["Pause"];
        if (pauseAction == null)
        {
            return;
        }

        pauseAction.Enable();
        pauseAction.performed += OnPausePressed;

        cancelAction = playerInput.actions["Cancel"];
        if (cancelAction == null)
        {
            return;
        }

        cancelAction.Enable();
        cancelAction.performed += OnCancelPressed;
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
        cancelAction.performed -= OnPausePressed;
        cancelAction.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }
    
    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (optionsUI.activeSelf)
        {
            optionsUI.SetActive(false);
            pauseUI.SetActive(true);
            return;
        }
        
        if (pauseUI.activeSelf)
        {
            DeactivateMenu();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            DeactivateMenu();
        }
        else
        {
            if (Time.timeScale == 0)
            {
                return;
            }
            ActivateMenu();   
        }
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);
        isPaused = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        isPaused = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGame() => TogglePause();

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
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        optionsUI.SetActive(false);
        pauseUI.SetActive(true);
    }
}
