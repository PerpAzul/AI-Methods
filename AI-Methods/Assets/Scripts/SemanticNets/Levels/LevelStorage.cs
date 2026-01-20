using UnityEngine;
using System.Collections.Generic;

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage Instance;

    public List<(string, string, int)> level0Edges;
    public List<(string, string, int)> level1Edges;
    public List<(string, string, int)> level2Edges;

    public List<(string, string, int)>[] levels;

    public string[] meaning;

    // IMPORTANT: when adding a new level, add a new list here and initialize is in Awake()
    // IMPORTANT: the edges have to be unidirectional for now

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;

        level0Edges = new List<(string, string, int)> {
            ("Essen", "Obst", 0),
            ("Essen", "Gemüse", 0),
            ("Obst", "Apfel", 0)
        };

        level1Edges = new List<(string, string, int)> {
            ("Wissenschaft", "Ingenieurwissenschaft", 0),
            ("Wissenschaft", "Naturwissenschaft", 0),
            ("Naturwissenschaft", "Physik", 0),
            ("Naturwissenschaft", "Chemie", 0),
            ("Ingenieurwissenschaft", "Informatik", 0),
            ("Ingenieurwissenschaft", "Maschinenbau", 0)
        };

        level2Edges = new List<(string, string, int)> {
            ("Tier", "Säugetier", 0),
            ("Tier", "Fisch", 0),
            ("Tier", "Vogel", 0),
            ("Zellen", "Tier", 1),

            ("Säugetier", "Hund", 0),
            ("Säugetier", "Mensch", 0),

            ("Bellen", "Hund", 2),
            ("Programmieren", "Mensch", 2),

            ("Fisch", "Hai", 0),
            ("Kiemen", "Fisch", 1),
            ("Schwimmen", "Fisch", 2),

            ("Vogel", "Adler", 0),
            ("Flügel", "Vogel", 1),
            ("Fliegen", "Vogel", 2)
        };

        // more levels to be initialized here
        levels = new List<(string, string, int)>[]{
            level0Edges,
            level1Edges,
            level2Edges
        };

        meaning = new string[]{
            "ist",
            "hat",
            "kann"
        };
    }

    public bool containsEdge(string node1, string node2, int type, int level) {
        node1 = Normalize(node1);
        node2 = Normalize(node2);

        foreach (var edge in levels[level]) {
            if (Normalize(edge.Item1) == node1 && Normalize(edge.Item2) == node2 && edge.Item3 == type) 
                return true;
            if (Normalize(edge.Item2) == node1 && Normalize(edge.Item1) == node2 && edge.Item3 == type)
                return true;
        }
        return false;
    }

    public bool containsTransitiveEdge(string node1, string node2, int type, int level) {
        node1 = Normalize(node1);
        node2 = Normalize(node2);

        string curNode = node2;
        string oneOccurence = "";
        while (true) {
            bool found = false;
            foreach (var edge in levels[level]) {
                if ((Normalize(edge.Item2) == curNode && edge.Item3 == 0)
                    || (Normalize(edge.Item2) == curNode && edge.Item3 == type && edge.Item1 == node1)) {
                    oneOccurence = edge.Item1;
                    if (edge.Item1 == node1 && edge.Item3 == type) {
                        return true;
                    }
                    found = true;
                }
            }
            if (!found) {
                break;
            }
            curNode = oneOccurence;
        }

        // same thing for case not unidirectional player input
        curNode = node1;
        while (true) {
            bool found = false;
            foreach (var edge in levels[level]) {
                if ((Normalize(edge.Item2) == curNode && edge.Item3 == 0)
                    || (Normalize(edge.Item2) == curNode && edge.Item3 == type && edge.Item1 == node2)) {
                    oneOccurence = edge.Item1;
                    if (edge.Item1 == node2 && edge.Item3 == type) {
                        return true;
                    }
                    found = true;
                }
            }
            if (!found) {
                return false;
            }
            curNode = oneOccurence;
        }
    }

    string Normalize(string s) =>
        s.Normalize(System.Text.NormalizationForm.FormC).Trim();

    public (string, string)? getPair(string node1, string node2, int level) {
        List<(string, string, int)> correctEdges = levels[level];
        foreach (var edge in correctEdges) {
            if ((Normalize(edge.Item1) == node1 && Normalize(edge.Item2) == node2)
                || (Normalize(edge.Item2) == node1 && Normalize(edge.Item1) == node2)) {
                    return (edge.Item1, edge.Item2);
                }
        }
        return null;
    }
}
