using UnityEngine;

public class ZigZagLine : Line
{
    [Header("ZigZag Settings")]
    public float zigZagSpacing = 0.5f;   // Distance between zigzag points
    public float zigZagAmplitude = 0.2f; // Sideways offset

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    protected override void Update()
    {
        if (lr.positionCount < 2)
            return;
        
        if (start != null && end != null) {
            DrawZigZag();
        }
    }

    void DrawZigZag()
    {
        Vector3 direction = end.position - start.position;
        float length = direction.magnitude;
        direction.Normalize();
        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(direction.x, direction.z));
        float xAmplitude = (float) Mathf.Cos(angle * Mathf.PI / 180) * zigZagAmplitude;
        float zAmplitude = (float) Mathf.Sin(angle * Mathf.PI / 180) * zigZagAmplitude;

        int pointsCount = Mathf.FloorToInt(length / zigZagSpacing);
        lr.positionCount = pointsCount + 1;

        for (int i = 0; i < lr.positionCount; i++) {
            Vector3 point = Vector3.Lerp(start.position, end.position, (i * zigZagSpacing) / length);
            point.x -= (i % 2 == 0 ? 1 : -1) * xAmplitude;
            point.z -= (i % 2 == 0 ? 1 : -1) * zAmplitude;
            lr.SetPosition(i, point);
        }

        int sign = lr.positionCount % 2 == 0 ? 1 : -1;
        lr.positionCount++;
        Vector3 pointEnd = Vector3.Lerp(start.position, end.position, 1);
        float lastAmplitude = sign * (zigZagAmplitude * 2 / zigZagSpacing) * (length % zigZagSpacing) - sign * zigZagAmplitude;
        pointEnd.x += (float) Mathf.Cos(angle * Mathf.PI / 180) * lastAmplitude;
        pointEnd.y -= (float) Mathf.Sin(angle * Mathf.PI / 180) * lastAmplitude;
        lr.SetPosition(lr.positionCount - 1, pointEnd);
    }
}
