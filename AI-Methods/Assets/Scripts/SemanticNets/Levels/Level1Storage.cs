using UnityEngine;
using System.Collections.Generic;

public class Level1Storage : MonoBehaviour
{
    public static Level1Storage Instance;

    public List<(string, string)> correctEdges;
    public List<(string, string)> missingEdges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        correctEdges = new List<(string, string)> {
            ("Wissenschaft", "Ingenieurswissenschaften"),
            ("Wissenschaft", "Naturwissenschaften"),
            ("Naturwissenschaften", "Physik"),
            ("Naturwissenschaften", "Chemie"),
            ("Ingenieurswissenschaften", "Informatik"),
            ("Ingenieurswissenschaften", "Maschinenbau")
        };
        missingEdges = new List<(string, string)>{};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool containsEdge(string node1, string node2) {
        foreach (var edge in correctEdges) {
            if ((edge.Item1 == node1 && edge.Item2 == node2) || (edge.Item1 == node2 && edge.Item2 == node1)) {
                return true;
            }
        }
        return false;
    }
}
