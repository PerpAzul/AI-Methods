using UnityEngine;
using System.Collections.Generic;

public class Level1Storage : MonoBehaviour
{
    public static Level1Storage Instance;

    public List<(string, string)> edges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        edges = new List<(string, string)> {
            ("Wissenschaft", "Ingenieurswissenschaften"),
            ("Wissenschaft", "Naturwissenschaften"),
            ("Naturwissenschaften", "Physik"),
            ("Naturwissenschaften", "Chemie"),
            ("Ingenieurswissenschaften", "Informatik"),
            ("Ingenieurswissenschaften", "Maschinenbau")
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
