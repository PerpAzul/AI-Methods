using UnityEngine;

public class LineObject : InteractableI
{
    private Transform startNode;
    private Transform endNode;
    private string earlierMessage;

    public void Init(Transform startNode, Transform endNode)
    {
        this.startNode = startNode;
        this.endNode = endNode;
    }
    
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
        LevelManager.Instance.removeEdge(startNode, endNode);
        Destroy(this.gameObject);
    }

    public Transform getStartNode()
    {
        return startNode;
    }

    public Transform getEndNode()
    {
        return endNode;
    }
}
