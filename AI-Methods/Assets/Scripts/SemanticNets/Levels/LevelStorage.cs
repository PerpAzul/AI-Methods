using UnityEngine;
using System.Collections.Generic;

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage Instance;

    public List<(string, string)> level0Edges;
    public List<(string, string)> level1Edges;
    public List<(string, string)> level2Edges;

    public List<(string, string)>[] levels;

    // IMPORTANT: when adding a new level, add a new list here and initialize is in Awake()

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;
        DontDestroyOnLoad(gameObject);

        level0Edges = new List<(string, string)> {
            ("Sprache", "Deutsch"),
            ("Sprache", "Englisch")
        };

        level1Edges = new List<(string, string)> {
            ("Wissenschaft", "Ingenieurswissenschaften"),
            ("Wissenschaft", "Naturwissenschaften"),
            ("Naturwissenschaften", "Physik"),
            ("Naturwissenschaften", "Chemie"),
            ("Ingenieurswissenschaften", "Informatik"),
            ("Ingenieurswissenschaften", "Maschinenbau")
        };

        level2Edges = new List<(string, string)> {
            ("Informatik", "IT-Sicherheit"),
            ("Informatik", "Algorithmen")
        };

        // more levels to be initialized here
        levels = new List<(string, string)>[]{
            level0Edges,
            level1Edges,
            level2Edges
        };
        // put more levels into the array (used for easier access)
    }

    public bool containsEdge(string node1, string node2, int level) {
        List<(string, string)> correctEdges = levels[level];
        foreach (var edge in correctEdges) {
            if ((edge.Item1 == node1 && edge.Item2 == node2) || (edge.Item1 == node2 && edge.Item2 == node1)) {
                return true;
            }
        }
        return false;
    }
}
