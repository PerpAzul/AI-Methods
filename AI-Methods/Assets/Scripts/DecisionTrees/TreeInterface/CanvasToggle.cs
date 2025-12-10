using System;
using Unity.Cinemachine;
using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    public Canvas fullscreenCanvas;
    public Transform worldPanelParent;
    public Transform overlayPanelParent;
    public RectTransform uiTree;
    public CinemachineCamera myCamera;
    public NPCGuide guide;
    private ToggleBookButtonUI toggleBookButtonUI;

    private bool isPlayerNear;

    private void Start()
    {
        uiTree.SetParent((worldPanelParent));
        uiTree.anchoredPosition = Vector2.zero;
        uiTree.localRotation = Quaternion.identity;
        uiTree.localScale = Vector3.one;
        toggleBookButtonUI = GameObject.Find("Book UIs").GetComponent<ToggleBookButtonUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
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

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !fullscreenCanvas.gameObject.activeSelf)
        {
            if (guide) guide.ContinueIfCurrentActionEquals("dtui_e");
            toggleBookButtonUI.treeOpen = true;
            uiTree.SetParent(overlayPanelParent);
            uiTree.anchoredPosition = Vector2.zero;
            uiTree.localRotation = Quaternion.identity;
            uiTree.localScale = Vector3.one;
            
            fullscreenCanvas.gameObject.SetActive(true);
            myCamera.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else if (Input.GetKeyDown(KeyCode.E) && fullscreenCanvas.gameObject.activeSelf)
        {
            toggleBookButtonUI.treeOpen = false;
            uiTree.SetParent((worldPanelParent));
            uiTree.anchoredPosition = Vector2.zero;
            uiTree.localRotation = Quaternion.identity;
            uiTree.localScale = Vector3.one;
            fullscreenCanvas.gameObject.SetActive(false);
            myCamera.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } 
    }

}
