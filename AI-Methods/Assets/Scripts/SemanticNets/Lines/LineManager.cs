using UnityEngine;
using TMPro;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;  // Singleton für einfachen Zugriff

    [SerializeField] private Line[] lines;
    private Line currentLine;
    private Transform firstTarget;
    private Transform secondTarget;
    private bool waitingForSecond = false;

    [Header("UI for Line")]
    [SerializeField] private GameObject infoCanvas;

    // NEW: remember the currently selected line type while a line is "attached" to player
    public int CurrentTypeIdx { get; private set; } = 0;

    // NEW: locks the line type picked on the FIRST click until the edge is finalized/cancelled
    private int pendingIdx = 0;

    void Awake()
    {
        Instance = this;
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(false);
        }
    }

    // NEW: expose locked type for scripts that need to enforce "same key on 2nd click"
    public int GetPendingIdx() => pendingIdx;

    // NEW: cancel dangling player-attached line (e.g. pressed key/E with no interactable)
    public void CancelCurrentLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }

        waitingForSecond = false;
        currentLine = null;
        firstTarget = null;
        secondTarget = null;

        // keep CurrentTypeIdx / pendingIdx as last chosen (optional; feels nice for re-trying)
    }

    // Optional convenience (kept from your version)
    public void ConnectToUsingCurrentType(Transform newTarget, Transform player)
    {
        ConnectTo(newTarget, player, CurrentTypeIdx);
    }

    public void ConnectTo(Transform newTarget, Transform player, int idx)
    {
        if (lines == null || lines.Length == 0) return;

        // clamp idx to valid range
        idx = Mathf.Clamp(idx, 0, lines.Length - 1);

        // check if max. edge amount reached
        if (LevelManager.Instance.HasReachedMaxEdges())
        {
            CancelCurrentLine();
            return;
        }

        if (!waitingForSecond)
        {
            // FIRST CLICK: Start line (player -> object)
            waitingForSecond = true;

            // lock chosen type now
            pendingIdx = idx;
            CurrentTypeIdx = idx;

            if (currentLine != null) Destroy(currentLine.gameObject); // alte Linie löschen

            // start preview line: object to player
            CreateLine(newTarget, player, pendingIdx);

            firstTarget = player;
            secondTarget = newTarget;
        }
        else
        {
            // SECOND CLICK: finalize edge (previous object -> new object)
            waitingForSecond = false;

            // IMPORTANT: ignore incoming idx; always use locked pendingIdx
            int lockedIdx = pendingIdx;
            CurrentTypeIdx = lockedIdx;

            firstTarget = secondTarget;
            secondTarget = newTarget;

            if (currentLine != null) Destroy(currentLine.gameObject); // alte Linie löschen

            if (LevelManager.Instance.isValidConnection(firstTarget, secondTarget))
            {
                CreateLine(firstTarget, secondTarget, lockedIdx);
                AddCollider(currentLine.gameObject, firstTarget, secondTarget, lockedIdx);
                LevelManager.Instance.addEdge(firstTarget, secondTarget, lockedIdx);
            }

            currentLine = null;
        }
    }

    private void CreateLine(Transform start, Transform end, int idx)
    {
        if (idx < 0 || idx >= lines.Length || lines[idx] == null) return;

        currentLine = Instantiate(lines[idx], lines[idx].transform.position, lines[idx].transform.rotation);
        currentLine.gameObject.SetActive(true);

        if (waitingForSecond)
        {
            currentLine.SetColor(Color.cyan);
        }
        else
        {
            currentLine.SetColor(LevelManager.Instance.GetLineColor(start, end, idx));
        }

        currentLine.SetStart(start);
        currentLine.SetEnd(end);

        currentLine.transform.position = (end.position + start.position) / 2;
    }

    private void AddCollider(GameObject lineObj, Transform startNode, Transform endNode, int idx)
    {
        Vector3 start = startNode.position;
        Vector3 end = endNode.position;

        CapsuleCollider col = lineObj.AddComponent<CapsuleCollider>();
        col.isTrigger = true;

        if ((end - start).magnitude > 3)
            col.height = (end - start).magnitude - 3f;
        else
            col.height = 0.1f;

        col.radius = 1f;
        col.transform.localRotation = Quaternion.FromToRotation(Vector3.up, (end - start).normalized);
        col.center = Vector3.zero;

        lineObj.layer = LayerMask.NameToLayer("Interactable");

        LineObject lineObjComponent = lineObj.AddComponent<LineObject>();
        lineObjComponent.Init(startNode, endNode, idx);

        if (infoCanvas != null)
            lineObjComponent.infoCanvas = infoCanvas;
    }

    public bool onPlayer()
    {
        return waitingForSecond;
    }
}
