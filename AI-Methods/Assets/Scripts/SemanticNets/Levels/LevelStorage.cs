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
            ("Tier", "Wirbellose", 0),
            ("Tier", "Wirbeltier", 0),
            ("Wirbellose", "Insekt", 0),
            ("Wirbellose", "Weichtier", 0),
            ("Wirbeltier", "Fisch", 0),
            ("Wirbeltier", "Amphibie", 0),
            ("Insekt", "Käfer", 0),
            ("Weichtier", "Muschel", 0),
            ("Fisch", "Lachs", 0),
            ("Amphibie", "Frosch", 0)
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
        while (true) {
            bool found = false;
            foreach (var edge in levels[level]) {
                if (Normalize(edge.Item2) == curNode && edge.Item3 == type) {
                    curNode = edge.Item1;
                    if (curNode == node1) {
                        return true;
                    }
                    found = true;
                    break;
                }
            }
            if (!found) {
                break;
            }
        }

        // same thing for case not unidirectional player input
        curNode = node1;
        while (true) {
            bool found = false;
            foreach (var edge in levels[level]) {
                if (Normalize(edge.Item2) == curNode && edge.Item3 == type) {
                    curNode = edge.Item1;
                    if (curNode == node2) {
                        return true;
                    }
                    found = true;
                    break;
                }
            }
            if (!found) {
                return false;
            }
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
