using UnityEngine;
using TMPro;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;  // Singleton für einfachen Zugriff

    [SerializeField] private Material lineMaterial;
    [SerializeField] private Line[] lines;
    private Line currentLine;
    private Transform firstTarget;
    private Transform secondTarget;
    private bool waitingForSecond = false;

    [Header("UI for Line")]
    [SerializeField] private GameObject infoCanvas;

    void Awake()
    {
        Instance = this;
        if (infoCanvas != null) {
            infoCanvas.SetActive(false);
        }
    }

    public void ConnectTo(Transform newTarget, Transform player, int idx)
    {
        if (idx >= lines.Length) {
            return;
        }
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
            CreateLine(newTarget, player, idx);
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
                CreateLine(firstTarget, secondTarget, idx);
                AddCollider(currentLine.gameObject, firstTarget, secondTarget);
                LevelManager.Instance.addEdge(firstTarget, secondTarget);
            }
            currentLine = null;
        }
    }

    private void CreateLine(Transform start, Transform end, int idx)
    {
        currentLine = Instantiate(lines[idx], lines[idx].transform.position, lines[idx].transform.rotation);

        if (waitingForSecond) {
            currentLine.SetColor(Color.cyan);
        } else {
            currentLine.SetColor(LevelManager.Instance.GetLineColor(start, end));
        }

        currentLine.SetStart(start);
        currentLine.SetEnd(end);

        currentLine.transform.position = (end.position + start.position) / 2;
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
        col.center = Vector3.zero;

        lineObj.layer = LayerMask.NameToLayer("Interactable");
        LineObject lineObjComponent = lineObj.AddComponent<LineObject>();
        lineObjComponent.Init(startNode, endNode);
        if (infoCanvas != null) {
            lineObjComponent.infoCanvas = infoCanvas;
        }
    }

    public bool onPlayer()
    {
        return waitingForSecond;
    }
}
