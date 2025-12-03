using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 0.5f;   // How high it bounces
    public float frequency = 1f;     // How fast it bounces

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Bounce along Y axis
        float yOffset = Mathf.Sin(Time.time * frequency * 2 * Mathf.PI) * amplitude;
        transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }
}

