using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class BookToggle : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;
    BookUntoggle bookUntoggle;
    public GameObject locked;

    public NPCGuide guide;
    
    public PlayerProximity playerProximity;
    private FindHelper findHelper;
    private GameObject hintCanvas;

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
        bookUntoggle = GameObject.Find("Book UIs").transform.Find(fullscreenCanvas.name).GetComponent<BookUntoggle>();
        findHelper = GameObject.Find("GameManager").GetComponent<FindHelper>();
        hintCanvas = GameObject.Find("HintCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        // make sure you can still interact with ui after pausing
        if (fullscreenCanvas.gameObject.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            if (this.name.Equals("book_0001b"))
            {
                findHelper.find(0);
            }
            if (this.name.Equals("book_0001d"))
            {
                findHelper.find(1);
            }
            if(guide) guide.ContinueIfCurrentActionEquals("book_e");
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
