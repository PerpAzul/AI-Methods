using UnityEngine;
using System.Collections;

public class LineObject : InteractableI
{
    private Transform startNode;
    private Transform endNode;
    private int type;
    private string earlierMessage;
    private bool playerInRange = false;
    private bool correctEdge;

    [SerializeField] public GameObject infoCanvas;

    public void Init(Transform startNode, Transform endNode, int type)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.type = type;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctEdge = LevelManager.Instance.isCorrectConnection(startNode, endNode, type);
        if (correctEdge) {
            promptMessage = "";
            Destroy(GetComponent<CapsuleCollider>());
        } else {
            promptMessage = "<size=58%>Löschen (Q)</size>";
            if (LevelManager.Instance.isNeutralConnection(startNode, endNode, type)) {
                promptMessage = "<size=70%>Es gibt eine bessere Lösung.(Tipp: [E])</size>\n" + promptMessage;
            }
        }
        earlierMessage = promptMessage;
        if (infoCanvas != null) {
            infoCanvas.SetActive(false);
        }
    }

    protected override void Interact()
    {
        if (correctEdge) {
            return; // do nothing for correct edges
        }
        if (LevelManager.Instance.isNeutralConnection(startNode, endNode, type)) {
            if (infoCanvas != null) {
                infoCanvas.SetActive(!infoCanvas.activeSelf);
                infoCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = "Deine Verbindung " 
                    + startNode.GetChild(0).GetComponent<TMPro.TMP_Text>().text + " - "
                    + endNode.GetChild(0).GetComponent<TMPro.TMP_Text>().text + " ist zwar korrekt, aber noch nicht perfekt.\n"
                    + "Damit das Netz nicht zu unübersichtlich wird, solltest du versuchen, "
                    + startNode.GetChild(0).GetComponent<TMPro.TMP_Text>().text + " und "
                    + endNode.GetChild(0).GetComponent<TMPro.TMP_Text>().text + " über einen oder mehrere Knoten dazwischen zu verbinden.";
                StartCoroutine(DisableAfterSeconds(8f));
            }
        }
    }

    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (infoCanvas != null) {
            infoCanvas.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (infoCanvas != null) {
            infoCanvas.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LineManager.Instance != null && playerInRange && !LineManager.Instance.onPlayer())
        {
            promptMessage = earlierMessage;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!correctEdge) {
                    LevelManager.Instance.removeEdge(startNode, endNode, type);
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
