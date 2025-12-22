using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;

    protected LineRenderer lr;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    protected virtual void Update() {
        if (start != null && end != null) {
            lr.SetPosition(0, start.position);
            lr.SetPosition(1, end.position);
        }
    }

    public Transform GetStart() {
        return start;
    }

    public Transform GetEnd() {
        return end;
    }

    public void SetStart(Transform start) {
        this.start = start;
    }

    public void SetEnd(Transform end) {
        this.end = end;
    }

    public virtual void SetColor(Color color) {
        lr.material.color = color;
    }
}
