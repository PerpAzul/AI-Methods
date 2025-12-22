using UnityEngine;
using TMPro;

public class ArtObject : InteractableI
{

    private string originalPrompt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (transform.childCount > 0) {
            Transform child = transform.GetChild(0);
            TMP_Text tmp = child.GetComponent<TextMeshPro>();
            originalPrompt = tmp.text;
            promptMessage = originalPrompt;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If not exceeding max edges, update back to original title
        if (!LevelManager.Instance.HasReachedMaxEdges())
        {
            promptMessage = originalPrompt;
        }
    }

    protected override void Interact()
    {
        alreadyInteracted = true;
        Debug.Log("Connecting art object to player in semantic net.");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // If max edges reached, modify popup
        if (LevelManager.Instance.HasReachedMaxEdges()) {
            promptMessage = 
                "Maximale Anzahl an Kanten erreicht!\n" +
                "<size=80%>Lösche mindestens eine bestehende Kante, um weiterzuzeichnen.</size>";
            return; 
        }

        if (LineManager.Instance != null && player != null)
        {
            LineManager.Instance.ConnectTo(transform, player.transform, 2);
        }
    }
}
