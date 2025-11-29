using UnityEngine;

public class LineObject : InteractableI
{
    private Transform startNode;
    private Transform endNode;
    private string earlierMessage;
    private bool playerInRange = false;

    public void Init(Transform startNode, Transform endNode)
    {
        this.startNode = startNode;
        this.endNode = endNode;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        promptMessage = "Drücke Q zum Löschen";
        earlierMessage = promptMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if (LineManager.Instance != null && playerInRange && !LineManager.Instance.onPlayer())
        {
            promptMessage = earlierMessage;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LevelManager.Instance.removeEdge(startNode, endNode);
                Destroy(this.gameObject);
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
