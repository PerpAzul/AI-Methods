using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class BookToggle : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public CinemachineCamera myCamera;
    BookUntoggle bookUntoggle;
    public GameObject locked; 

    private bool isNear;
    void OnTriggerEnter(Collider other)
    {
        isNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        isNear = false;
    }
    
    void Start()
    {
        bookUntoggle = GameObject.Find("Book UIs").transform.Find(fullscreenCanvas.name).GetComponent<BookUntoggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            locked.SetActive(false);
            bookUntoggle.isUnlocked = true;
            fullscreenCanvas.gameObject.SetActive(true);
            myCamera.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(gameObject);
        }
    }
}
