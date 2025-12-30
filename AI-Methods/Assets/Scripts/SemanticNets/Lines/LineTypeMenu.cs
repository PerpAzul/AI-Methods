using System;
using TMPro;
using UnityEngine;

public class LineTypeMenu : MonoBehaviour
{
    [Header("UI (set text in Editor, not in code)")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text optionsText;
    [SerializeField] private TMP_Text hintText;

    [Header("Blocking movement")]
    [Tooltip("If true, pauses the whole game while menu is open (simple & reliable).")]
    [SerializeField] private bool pauseTime = false;

    [Tooltip("Player movement component type name to disable while menu is open (e.g. PlayerMovement).")]
    [SerializeField] private string movementComponentName = "PlayerMovement";

    private bool isOpen = false;
    private Action<int> onPick;

    private float prevTimeScale;
    private CursorLockMode prevLockState;
    private bool prevCursorVisible;

    private Behaviour cachedMovement;

    public int LastPickedIdx { get; private set; } = 0;
    public bool HasPickedAtLeastOnce { get; private set; } = false;

    private void Awake()
    {
        // Only controls visibility. Text is authored in the Editor.
        gameObject.SetActive(false);
    }

    public bool IsOpen => isOpen;

    public void Open(Action<int> onPickCallback)
    {
        if (isOpen) return;

        isOpen = true;
        onPick = onPickCallback;

        // show UI (your Editor-authored text stays as-is)
        gameObject.SetActive(true);

        // cursor for UI
        prevLockState = Cursor.lockState;
        prevCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // block movement
        if (pauseTime)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            if (cachedMovement == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    cachedMovement = player.GetComponent(movementComponentName) as Behaviour;
            }

            if (cachedMovement != null)
                cachedMovement.enabled = false;
        }
    }

    private void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            Pick(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            Pick(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            Pick(2);
        else if (Input.GetKeyDown(KeyCode.R))
            Close(); // close without picking
    }

    private void Pick(int idx)
    {
        LastPickedIdx = idx;
        HasPickedAtLeastOnce = true;

        var cb = onPick;
        Close();
        cb?.Invoke(idx);
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        onPick = null;

        if (pauseTime)
            Time.timeScale = prevTimeScale;
        else if (cachedMovement != null)
            cachedMovement.enabled = true;

        Cursor.lockState = prevLockState;
        Cursor.visible = prevCursorVisible;

        gameObject.SetActive(false);
    }
}
