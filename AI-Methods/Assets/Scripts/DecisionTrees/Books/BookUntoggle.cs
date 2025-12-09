using Unity.Cinemachine;
using UnityEngine;

public class BookUntoggle : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] CinemachineCamera cam;

    public bool isUnlocked = false;
    private ToggleBookButtonUI toggleBookButtonUI;

    void Start()
    {
        toggleBookButtonUI = GameObject.Find("Book UIs").GetComponent<ToggleBookButtonUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canvas.gameObject.activeSelf)
        {
            toggleBookButtonUI.bookOpen = false;
            canvas.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
        }
    }
}

