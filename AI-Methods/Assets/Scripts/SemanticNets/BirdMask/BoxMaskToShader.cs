using UnityEngine;

public class BoxMaskToShader : MonoBehaviour
{
    public BoxCollider box;

    void Update()
    {
        Transform t = box.transform;

        Shader.SetGlobalVector("_BoxCenter", box.bounds.center);
        Shader.SetGlobalVector("_BoxSize", box.bounds.size);
        Shader.SetGlobalMatrix("_BoxMatrix", t.worldToLocalMatrix);
    }
}
