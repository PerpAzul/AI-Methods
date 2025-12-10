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
    // IMPORTANT: the edges have to be unidirectional for now

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;

        level0Edges = new List<(string, string)> {
            ("Essen", "Obst"),
            ("Essen", "Gemüse"),
            ("Obst", "Apfel")
        };

        level1Edges = new List<(string, string)> {
            ("Wissenschaft", "Ingenieurwissenschaft"),
            ("Wissenschaft", "Naturwissenschaft"),
            ("Naturwissenschaft", "Physik"),
            ("Naturwissenschaft", "Chemie"),
            ("Ingenieurwissenschaft", "Informatik"),
            ("Ingenieurwissenschaft", "Maschinenbau")
        };

        level2Edges = new List<(string, string)> {
            ("Tier", "Wirbellose"),
            ("Tier", "Wirbeltier"),
            ("Wirbellose", "Insekt"),
            ("Wirbellose", "Weichtier"),
            ("Wirbeltier", "Fisch"),
            ("Wirbeltier", "Amphibie"),
            ("Insekt", "Käfer"),
            ("Weichtier", "Muschel"),
            ("Fisch", "Lachs"),
            ("Amphibie", "Frosch")
        };

        // more levels to be initialized here
        levels = new List<(string, string)>[]{
            level0Edges,
            level1Edges,
            level2Edges
        };
    }

    public bool containsEdge(string node1, string node2, int level) {
        node1 = Normalize(node1);
        node2 = Normalize(node2);

        foreach (var edge in levels[level]) {
            if (Normalize(edge.Item1) == node1 && Normalize(edge.Item2) == node2) 
                return true;
            if (Normalize(edge.Item2) == node1 && Normalize(edge.Item1) == node2)
                return true;
        }
        return false;
    }

    public bool containsTransitiveEdge(string node1, string node2, int level) {
        node1 = Normalize(node1);
        node2 = Normalize(node2);

        string curNode = node2;
        while (true) {
            bool found = false;
            foreach (var edge in levels[level]) {
                if (Normalize(edge.Item2) == curNode) {
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
                if (edge.Item2 == curNode) {
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
        List<(string, string)> correctEdges = levels[level];
        foreach (var edge in correctEdges) {
            if ((Normalize(edge.Item1) == node1 && Normalize(edge.Item2) == node2)
                || (Normalize(edge.Item2) == node1 && Normalize(edge.Item1) == node2)) {
                    return edge;
                }
        }
        return null;
    }
}
