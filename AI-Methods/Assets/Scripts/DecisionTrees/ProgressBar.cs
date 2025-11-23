using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image filler;
    float max = 8.0f;
    public float curr = 0.0f; 

    // Update is called once per frame
    void Update()
    {
        setFill();
    }

    void setFill()
    {
        Vector3 newScale = filler.rectTransform.localScale;
        newScale.x = curr / max;
        filler.rectTransform.localScale = newScale;
    }
}
