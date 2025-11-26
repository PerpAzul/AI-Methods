using UnityEngine;

public class MiniMapZoomToggle : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform borderRect;
    public RectTransform miniMapRect;

    [Header("Zoom Settings")]
    public float zoomScale = 2f;
    public KeyCode holdKey = KeyCode.T;

    private Vector2 borderOriginalPos;
    private Vector2 miniOriginalPos;
    private Vector3 borderOriginalScale;
    private Vector3 miniOriginalScale;

    private RectTransform canvasRect;

    void Start()
    {
        borderOriginalPos = borderRect.anchoredPosition;
        miniOriginalPos   = miniMapRect.anchoredPosition;

        borderOriginalScale = borderRect.localScale;
        miniOriginalScale   = miniMapRect.localScale;

        canvasRect = borderRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetKey(holdKey))
        {
            ZoomToCenter();
        }
        else
        {
            Restore();
        }
    }

    private void ZoomToCenter()
    {
        Vector2 canvasCenter = new Vector2(0, 0);

        Vector2 centerPos =
            new Vector2(
                (canvasRect.rect.width  * (0.5f - borderRect.anchorMin.x)),
                (canvasRect.rect.height * (0.5f - borderRect.anchorMin.y))
            );

        borderRect.anchoredPosition = centerPos;
        miniMapRect.anchoredPosition = centerPos;

        borderRect.localScale = borderOriginalScale * zoomScale;
        miniMapRect.localScale = miniOriginalScale * zoomScale;
    }

    private void Restore()
    {
        borderRect.anchoredPosition = borderOriginalPos;
        miniMapRect.anchoredPosition = miniOriginalPos;

        borderRect.localScale = borderOriginalScale;
        miniMapRect.localScale = miniOriginalScale;
    }
}
