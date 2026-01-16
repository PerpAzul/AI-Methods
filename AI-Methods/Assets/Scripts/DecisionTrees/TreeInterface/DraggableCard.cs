using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    public bool ClearMe;

    private GameObject currentClone;
    private RectTransform cloneRect;
    private Canvas canvas;
    public Texture2D hoverCursor;

    public NPCGuide guide;

    [SerializeField] private List<DragAndDrop> toCheck;
    [SerializeField] private GameObject hoverWarning;
    private RectTransform hoverWarningRectTransform;

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
        if(hoverWarning)
            hoverWarningRectTransform = hoverWarning.GetComponent<RectTransform>();
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
            if(guide && targetText.text.Contains("Metall") && r.gameObject.name == "?_1_1") guide.ContinueIfCurrentActionEquals("metal_card");
            if(guide && targetText.text.Contains("Schädlich") && r.gameObject.name == "?_2_1") guide.ContinueIfCurrentActionEquals("danger_card");
            if(guide && targetText.text.Contains("Pilz") && r.gameObject.name == "?_1_1") guide.ContinueIfCurrentActionEquals("rot_card");
            if(guide && targetText.text.Contains("Rot") && r.gameObject.name == "?_2_1") guide.ContinueIfCurrentActionEquals("obst_card");
            if(guide && targetText.text.Contains("Rot") && r.gameObject.name == "?_2_2") guide.ContinueIfCurrentActionEquals("obst_card_2");
            break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myText.text != "?")
        {
            Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        hoverWarning.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        bool collision = false;
        foreach (var check in toCheck)
        {
            if (check.myText.text == myText.text && myText.text != "?")
            {
                collision = true;
            }
        }

        if (collision)
        {
            myText.color = new Color32(255, 0, 0, 255);
            hoverWarning.SetActive(true);
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                hoverWarningRectTransform.parent.GetComponent<RectTransform>(),
                eventData.position,
                null,
                out localPoint);
            hoverWarningRectTransform.localPosition = localPoint + new Vector2(300, 0);
        }
        else
        {
            myText.color = new Color32(255, 255, 255, 255);
        }
    }
}
