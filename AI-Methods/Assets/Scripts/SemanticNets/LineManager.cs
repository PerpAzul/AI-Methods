using UnityEngine;
using TMPro;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;  // Singleton für einfachen Zugriff

    private LineRenderer currentLine;
    private Transform firstTarget;
    private Transform secondTarget;
    private bool waitingForSecond = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Wenn es eine aktive Linie gibt, ihre Punkte updaten
        if (currentLine != null)
        {
            if (firstTarget != null)
                currentLine.SetPosition(0, firstTarget.position);

            if (secondTarget != null)
                currentLine.SetPosition(1, secondTarget.position);
        }
    }

    public void ConnectTo(Transform newTarget, Transform player)
    {
        // check if max. edge amount reached
        if (LevelManager.Instance.HasReachedMaxEdges()) {
            if (currentLine != null)
                Destroy(currentLine.gameObject);

            waitingForSecond = false;
            currentLine = null;
            return;
        }

        if (!waitingForSecond)
        {
            // Start: Spieler -> Objekt
            waitingForSecond = true;
            if (currentLine != null) Destroy(currentLine.gameObject); // alte Linie löschen
            CreateLine(player, newTarget);
            firstTarget = player;
            secondTarget = newTarget;
        }
        else
        {
            // Nächstes Objekt -> Vorheriges Objekt
            waitingForSecond = false;
            firstTarget = secondTarget;
            secondTarget = newTarget;
            Destroy(currentLine.gameObject); // alte Linie löschen
            if (LevelManager.Instance.isValidConnection(firstTarget, secondTarget)) {
                CreateLine(firstTarget, secondTarget);
                AddCollider(currentLine.gameObject, firstTarget, secondTarget);
                LevelManager.Instance.addEdge(firstTarget, secondTarget);
            }
            currentLine = null;
        }
    }

    private void CreateLine(Transform start, Transform end)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        currentLine = lineObj.AddComponent<LineRenderer>();

        currentLine.positionCount = 2;
        currentLine.startWidth = 0.05f;
        currentLine.endWidth = 0.05f;
        currentLine.material = new Material(Shader.Find("Unlit/Color"));

        if (waitingForSecond) {
            currentLine.material.color = Color.cyan;
        } else {
            currentLine.material.color = LevelManager.Instance.GetLineColor(start, end);
        }

        currentLine.SetPosition(0, start.position);
        currentLine.SetPosition(1, end.position);

        lineObj.transform.position = (end.position + start.position) / 2;

        lineObj.layer = LayerMask.NameToLayer("Interactable");
    }

    private void AddCollider(GameObject lineObj, Transform startNode, Transform endNode)
    {
        Vector3 start = startNode.position;
        Vector3 end = endNode.position;
        CapsuleCollider col = lineObj.AddComponent<CapsuleCollider>();
        col.isTrigger = true;
        if ((end - start).magnitude > 3) {
            col.height = (end - start).magnitude - 3f;
        } else {
            col.height = 0.1f;
        }
        col.radius = 1f;
        col.transform.localRotation = Quaternion.FromToRotation(Vector3.up, (end - start).normalized);

        LineObject lineObjComponent = lineObj.AddComponent<LineObject>();
        lineObjComponent.promptMessage = "Drücke E zum Löschen";
        lineObjComponent.Init(startNode, endNode);
    }

    public bool onPlayer()
    {
        return waitingForSecond;
    }
}
