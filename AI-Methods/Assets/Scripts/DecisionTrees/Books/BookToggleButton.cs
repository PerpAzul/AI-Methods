using UnityEngine;
using Unity.Cinemachine;

public class BookToggleButton : MonoBehaviour
{
    public GameObject bookui;
    public CinemachineCamera cam;
    ToggleBookButtonUI bookButtonUI;
    BookUntoggle bookUntoggle;

    public NPCGuide guide;

    // Update is called once per frame
    void Start()
    {
        bookButtonUI = GameObject.Find("Book UIs").GetComponent<ToggleBookButtonUI>();
        bookUntoggle = GameObject.Find("Book UIs").transform.Find(bookui.name).GetComponent<BookUntoggle>();
    }
    public void OpenUI()
    {
        if (bookUntoggle.isUnlocked)
        {
            if(guide) guide.ContinueIfCurrentActionEquals("book_q");
            bookButtonUI.bookOpen = true;
            bookButtonUI.isActive = false;
            bookButtonUI.buttonUI.SetActive(false);
            bookui.SetActive(true);
            cam.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
