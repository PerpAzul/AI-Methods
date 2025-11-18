using UnityEngine;

public class MiniMapZoomToggle : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform borderRect;      // White border panel
    public RectTransform miniMapRect;     // RawImage minimap

    [Header("Zoom Settings")]
    public float zoomScale = 2f;          // How much bigger the zoomed map is
    public Vector2 zoomPosition;          // Where enlarged map should appear
    public KeyCode holdKey = KeyCode.O;   // Hold key

    // Internal storage
    private Vector2 borderOriginalPos;
    private Vector2 miniOriginalPos;
    private Vector3 borderOriginalScale;
    private Vector3 miniOriginalScale;

    void Start()
    {
        // Save original transforms
        borderOriginalPos = borderRect.anchoredPosition;
        miniOriginalPos   = miniMapRect.anchoredPosition;

        borderOriginalScale = borderRect.localScale;
        miniOriginalScale   = miniMapRect.localScale;
    }

    void Update()
    {
        if (Input.GetKey(holdKey))
        {
            // HOLD → zoom in
            borderRect.anchoredPosition = zoomPosition;
            miniMapRect.anchoredPosition = zoomPosition;

            borderRect.localScale = borderOriginalScale * zoomScale;
            miniMapRect.localScale = miniOriginalScale * zoomScale;
        }
        else
        {
            // RELEASE → restore
            borderRect.anchoredPosition = borderOriginalPos;
            miniMapRect.anchoredPosition = miniOriginalPos;

            borderRect.localScale = borderOriginalScale;
            miniMapRect.localScale = miniOriginalScale;
        }
    }
}
