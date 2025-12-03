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

    private bool isPlayerNear;

    private void Start()
    {
        uiTree.SetParent((worldPanelParent));
        uiTree.anchoredPosition = Vector2.zero;
        uiTree.localRotation = Quaternion.identity;
        uiTree.localScale = Vector3.one;
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
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !fullscreenCanvas.gameObject.activeSelf)
        {
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
