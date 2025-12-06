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
            ("Tiere", "Wirbellose"),
            ("Tiere", "Wirbeltiere"),
            ("Wirbellose", "Insekten"),
            ("Wirbellose", "Weichtiere"),
            ("Wirbeltiere", "Fische"),
            ("Wirbeltiere", "Amphibien"),
            ("Insekten", "Käfer"),
            ("Weichtiere", "Muscheln"),
            ("Fische", "Tuna"),
            ("Amphibien", "Frösche")
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

string Normalize(string s) =>
    s.Normalize(System.Text.NormalizationForm.FormC).Trim();



    public (string, string)? getPair(string node1, string node2, int level) {
        List<(string, string)> correctEdges = levels[level];
        foreach (var edge in correctEdges) {
            if (edge.Item1 == node1 && edge.Item2 == node2) {
                return (node1, node2);
            }
            if  (edge.Item1 == node2 && edge.Item2 == node1) {
                return (node2, node1);
            }
        }
        return null;
    }
}
