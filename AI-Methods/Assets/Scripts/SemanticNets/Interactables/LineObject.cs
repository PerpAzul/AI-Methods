using UnityEngine;

public class LineObject : InteractableI
{
    private Transform startNode;
    private Transform endNode;
    private string earlierMessage;
    private bool playerInRange = false;
    private bool correctEdge;

    public void Init(Transform startNode, Transform endNode)
    {
        this.startNode = startNode;
        this.endNode = endNode;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctEdge = LevelManager.Instance.isCorrectConnection(startNode, endNode);
        if (correctEdge) {
            promptMessage = "";
        } else {
            promptMessage = "<size=50%>Löschen (Q)</size>";
        }
        earlierMessage = promptMessage;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<LineRenderer>().SetPosition(0, startNode.position);
        this.GetComponent<LineRenderer>().SetPosition(1, endNode.position);
        if (LineManager.Instance != null && playerInRange && !LineManager.Instance.onPlayer())
        {
            promptMessage = earlierMessage;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!correctEdge) {
                    LevelManager.Instance.removeEdge(startNode, endNode);
                    Destroy(this.gameObject);
                }
            }
        } else {
            promptMessage = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
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
