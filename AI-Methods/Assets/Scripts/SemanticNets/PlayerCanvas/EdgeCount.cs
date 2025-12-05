using UnityEngine;
using TMPro;

public class EdgeCount : MonoBehaviour
{
    private TextMeshProUGUI edgeCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        edgeCountText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        edgeCountText.text = "Kanten: " + LevelManager.Instance.getCorrectEdgesCount() + " / " + LevelManager.Instance.getMaxEdgesCount();
    }
}
