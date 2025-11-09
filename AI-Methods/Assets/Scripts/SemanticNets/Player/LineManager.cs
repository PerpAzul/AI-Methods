using UnityEngine;

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
        if (!waitingForSecond)
        {
            // Start: Spieler -> Objekt
            if (currentLine != null) Destroy(currentLine.gameObject); // alte Linie löschen
            CreateLine(player, newTarget);
            firstTarget = player;
            secondTarget = newTarget;
            waitingForSecond = true;
        }
        else
        {
            // Nächstes Objekt -> Vorheriges Objekt
            firstTarget = secondTarget;
            secondTarget = newTarget;
            Destroy(currentLine.gameObject); // alte Linie löschen
            if (firstTarget != secondTarget) {
                CreateLine(firstTarget, secondTarget);
                AddCollider(currentLine.gameObject, firstTarget.position, secondTarget.position);
            }
            currentLine = null;
            waitingForSecond = false;
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
        currentLine.material.color = Color.cyan;

        currentLine.SetPosition(0, start.position);
        currentLine.SetPosition(1, end.position);

        lineObj.transform.position = (end.position + start.position) / 2;

        lineObj.layer = LayerMask.NameToLayer("Interactable");
    }

    private void AddCollider(GameObject lineObj, Vector3 start, Vector3 end)
    {
        CapsuleCollider col = lineObj.AddComponent<CapsuleCollider>();
        col.isTrigger = true;
        col.height = (end - start).magnitude;
        col.radius = 0.03f;
        col.transform.localRotation = Quaternion.FromToRotation(Vector3.up, (end - start).normalized);

        Interactable interactable = lineObj.AddComponent<LineObject>();
        interactable.promptMessage = "Klicke zum Löschen";
    }
}