using UnityEngine;
using System.Collections.Generic;

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage Instance;

    public List<(string, string)> level1Edges;
    public List<(string, string)>[] levels;

    // IMPORTANT: when adding a new level, add a new list here and initialize is in Awake()

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        level1Edges = new List<(string, string)> {
            ("Wissenschaft", "Ingenieurswissenschaften"),
            ("Wissenschaft", "Naturwissenschaften"),
            ("Naturwissenschaften", "Physik"),
            ("Naturwissenschaften", "Chemie"),
            ("Ingenieurswissenschaften", "Informatik"),
            ("Ingenieurswissenschaften", "Maschinenbau")
        };
        // more levels to be initialized here

        levels = new List<(string, string)>[1];
        levels[0] = level1Edges;
        // put more levels into the array (used for easier access)
    }

    public bool containsEdge(string node1, string node2, int level) {
        List<(string, string)> correctEdges = levels[level-1];
        foreach (var edge in correctEdges) {
            if ((edge.Item1 == node1 && edge.Item2 == node2) || (edge.Item1 == node2 && edge.Item2 == node1)) {
                return true;
            }
        }
        return false;
    }
}
