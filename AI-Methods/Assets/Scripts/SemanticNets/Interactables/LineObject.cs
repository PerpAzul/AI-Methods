using UnityEngine;

public class LineObject : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Destroy()
    {
        Destroy(this.gameObject);
    }
}
