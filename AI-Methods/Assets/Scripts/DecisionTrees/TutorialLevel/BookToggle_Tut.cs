using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class BookToggle_Tut : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;
    BookUntoggle bookUntoggle;
    public GameObject locked;

    public NPCGuide guide;
    
    public PlayerProximity playerProximity;
    private GameObject hintCanvas;
    private ToggleBookButtonUI toggleBookButtonUI;

    private bool isNear;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
    
    void Start()
    {
        toggleBookButtonUI = GameObject.Find("Book UIs").GetComponent<ToggleBookButtonUI>();
        bookUntoggle = GameObject.Find("Book UIs").transform.Find(fullscreenCanvas.name).GetComponent<BookUntoggle>();
        hintCanvas = GameObject.Find("HintCanvas");
        this.GetComponent<HintDisplayer>().isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (guide && guide.canReadFood)
        {
            this.GetComponent<HintDisplayer>().isEnabled = true;
        }
        // make sure you can still interact with ui after pausing
        if (fullscreenCanvas.gameObject.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (isNear && Input.GetKeyDown(KeyCode.E) && guide.canReadFood)
        {
            if(guide) guide.ContinueIfCurrentActionEquals("book_e");
            toggleBookButtonUI.bookOpen = true;
            playerProximity.Hide();
            locked.SetActive(false);
            bookUntoggle.isUnlocked = true;
            fullscreenCanvas.gameObject.SetActive(true);
            myCamera.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            hintCanvas.GetComponent<Canvas>().enabled = false;
            Destroy(gameObject);
        }
    }
}
