using UnityEngine;
using TMPro;
using System.Collections;

public class ArtObjectLvl2 : InteractableI
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
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (LevelManager.Instance.HasReachedMaxEdges())
    {
        promptMessage =
            "Maximale Anzahl an Kanten erreicht!\n" +
            "<size=80%>Lösche mindestens eine bestehende Kante, um weiterzuzeichnen.</size>";
        return;
    }

    if (LineManager.Instance == null || player == null)
        return;

    LineTypeMenu menu = GameObject.FindObjectOfType<LineTypeMenu>(true);

    // If a line is currently attached to the player, DON'T open the menu again.
    // Just use the last picked idx (or 0 if never picked) and connect.
    if (LineManager.Instance.onPlayer())
    {
        int idxToUse = 0;
        if (menu != null && menu.HasPickedAtLeastOnce)
            idxToUse = menu.LastPickedIdx;

        LineManager.Instance.ConnectTo(transform, player.transform, idxToUse);
        return;
    }

    // First click: open the menu to choose type
    if (menu == null)
    {
        Debug.LogWarning("No LineTypeMenu found in scene. Falling back to default idx=0.");
        LineManager.Instance.ConnectTo(transform, player.transform, 0);
        return;
    }

    menu.Open((idx) =>
    {
        if (LineManager.Instance != null && player != null)
            LineManager.Instance.ConnectTo(transform, player.transform, idx);
    });
}



}
