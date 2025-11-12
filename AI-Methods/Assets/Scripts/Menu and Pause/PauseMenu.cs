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
    private bool isPaused;

    void Start()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput reference is missing on PauseMenu!");
            return;
        }

        pauseAction = playerInput.actions["Pause"];
        if (pauseAction == null)
        {
            Debug.LogError("No 'Pause' action found in PlayerInput actions.");
            return;
        }

        pauseAction.Enable();
        pauseAction.performed += OnPausePressed;

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }
        else
        {
            Debug.LogWarning("Back button not assigned in PauseMenu.");
        }
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
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
        
        //switch to UI map so Submit/Cancel/Navigate work
        if (playerInput != null && !string.IsNullOrEmpty(uiActionMap))
            playerInput.SwitchCurrentActionMap(uiActionMap);
        
        //free the cursor (mouse users)
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
        
        //back to gameplay input
        if (playerInput != null && !string.IsNullOrEmpty(gameplayActionMap))
            playerInput.SwitchCurrentActionMap(gameplayActionMap);

        //relock cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGame() => TogglePause();

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
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
