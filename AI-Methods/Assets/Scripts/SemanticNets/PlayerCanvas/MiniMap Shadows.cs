using UnityEngine;

public class DisableShadowsForCamera : MonoBehaviour
{
    float defaultShadowDistance;

    void OnPreRender()
    {
        defaultShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0f; // disable shadows
    }

    void OnPostRender()
    {
        QualitySettings.shadowDistance = defaultShadowDistance;
    }
}
