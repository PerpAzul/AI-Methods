using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int currentLevel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color GetLineColor(int level, string node1, string node2) {
        if (level == 1) {
            Debug.Log($"Checking connection between {node1} and {node2} in Level 1.");
            foreach (var edge in Level1Storage.Instance.edges) {
                if ((edge.Item1 == node1 && edge.Item2 == node2) || (edge.Item1 == node2 && edge.Item2 == node1)) {
                    return Color.green; // gültige Verbindung
                }
            }
            return Color.red; // ungültige Verbindung
        } else {
            return Color.cyan; // Standardfarbe für andere Level
        }
    }
}
