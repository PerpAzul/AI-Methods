using UnityEngine;
using TMPro;

public class ArtObject : InteractableI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.childCount > 0) {
            Transform child = transform.GetChild(0);
            TMP_Text tmp = child.GetComponent<TextMeshPro>();
            promptMessage = tmp.text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        Debug.Log("Connecting art object to player in semantic net.");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (LineManager.Instance != null && player != null)
        {
            LineManager.Instance.ConnectTo(transform, player.transform);
        }
    }
}
