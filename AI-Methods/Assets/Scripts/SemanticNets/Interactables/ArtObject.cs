using UnityEngine;

public class ArtObject : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (LineManager.Instance != null && player != null)
        {
            LineManager.Instance.ConnectTo(transform, player.transform);
        }
    }
}
