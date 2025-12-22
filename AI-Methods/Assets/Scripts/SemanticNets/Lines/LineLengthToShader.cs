using UnityEngine;

public class LineLengthToShader : Line
{
    MaterialPropertyBlock mpb;
    [SerializeField] private float dashLength = 0.05f;
    [SerializeField] private float gapLength = 0.03f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        mpb = new MaterialPropertyBlock();
        SetColor(Color.white);
        lr.GetPropertyBlock(mpb);
        mpb.SetFloat("_DashLength", dashLength);
        mpb.SetFloat("_GapLength", gapLength);
        lr.SetPropertyBlock(mpb);
    }

    void LateUpdate()
    {
        if (start == null || end == null) {
            return;
        }
        float length = Vector3.Distance(start.position, end.position);

        lr.GetPropertyBlock(mpb);
        mpb.SetFloat("_LineLength", length);
        lr.SetPropertyBlock(mpb);
    }

    public override void SetColor(Color color)
    {
        lr.GetPropertyBlock(mpb);
        mpb.SetColor("_LineColor", color);
        lr.SetPropertyBlock(mpb);
    }
}
