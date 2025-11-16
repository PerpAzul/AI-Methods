using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool ClearMe;

    private GameObject currentClone;
    private RectTransform cloneRect;
    private Canvas canvas;

    private Text myText;
    private Image myImage;
    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Kein Canvas im Parent gefunden");
        }
        myText = GetComponentInChildren<Text>();
        myImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (myText.text == "?")
        {
            return;
        }

        CreateClone(eventData);

        if (ClearMe)
        {
            myText.text = "?";
            myText.fontSize = 97;
            myImage.color = new Color32(175, 175, 255, 255);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentClone != null)
        {
            cloneRect.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentClone != null)
        {
            TryPlaceClone(eventData);
            Destroy(currentClone);
            currentClone = null;
        }
    }

    private void CreateClone(PointerEventData eventData)
    {
        currentClone = Instantiate(gameObject, canvas.transform);
        var cloneText = currentClone.GetComponentInChildren<Text>();
        cloneText.text = myText.text;
        cloneText.fontSize = myText.fontSize;

        cloneRect = currentClone.GetComponent<RectTransform>();
        cloneRect.position = eventData.position;

        var cloneImage = currentClone.GetComponent<Image>();
        cloneImage.color = new Color32(255, 255, 255, 170);
    }

    private void TryPlaceClone(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        // Reverse damit bei move out of droppable item nicht wieder das gleich item rauskommt
        results.Reverse();

        foreach (var r in results)
        {
            if (!r.gameObject.CompareTag("Droppable")) continue;

            var targetText = r.gameObject.GetComponent<Text>();
            var cloneText = currentClone.GetComponentInChildren<Text>();

            targetText.text = cloneText.text;
            targetText.fontSize = cloneText.fontSize;

            var targetImage = r.gameObject.GetComponentInParent<Image>();
            targetImage.color = new Color32(255, 255, 255, 255);
            break;
        }
    }
}
