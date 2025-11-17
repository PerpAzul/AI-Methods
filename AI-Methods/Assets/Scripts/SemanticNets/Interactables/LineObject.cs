using UnityEngine;

public class LineObject : InteractableI
{
    private string earlierMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        earlierMessage = promptMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if (LineManager.Instance != null && LineManager.Instance.onPlayer())
        {
            earlierMessage = promptMessage;
            promptMessage = "";
        } else
        {
            promptMessage = earlierMessage;
        }
    }

    protected override void Interact()
    {
        if (LineManager.Instance != null && LineManager.Instance.onPlayer())
        {
            return;
        }
        Destroy(this.gameObject);
    }
}
