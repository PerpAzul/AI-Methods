using UnityEngine;

public class ArtObject : InteractableI
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
        Debug.Log("Connecting art object to player in semantic net.");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (LineManager.Instance != null && player != null)
        {
            LineManager.Instance.ConnectTo(transform, player.transform);
        }
    }
}
