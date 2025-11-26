using UnityEngine;
using TMPro;

public class FloatingScoreText : MonoBehaviour
{
    public float lifetime = 1.2f;
    public float riseSpeed = 1f;
    public float fadeSpeed = 1f;

    private TextMeshPro text;

    void Awake()
    {
        text = gameObject.AddComponent<TextMeshPro>();
        text.fontSize = 4f;
        text.alignment = TextAlignmentOptions.Center;

        // Face camera
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    void Update()
    {
        lifetime -= Time.deltaTime;

        // Move upward
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        // Fade out
        Color c = text.color;
        c.a -= fadeSpeed * Time.deltaTime;
        text.color = c;

        if (lifetime <= 0f || text.color.a <= 0f)
            Destroy(gameObject);
    }

    public void SetText(string value, Color color)
    {
        text.text = value;
        color.a = 1f;
        text.color = color;
    }
}
