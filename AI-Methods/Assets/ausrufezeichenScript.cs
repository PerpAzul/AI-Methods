using UnityEngine;

public class ausrufezeichenScript : MonoBehaviour
{
    float yPos;
    bool onWayUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onWayUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        yPos = transform.position.y;
        if (onWayUp)
        {
            transform.position += new Vector3(0.0f, 0.2f, 0.0f) * Time.deltaTime;
        }
        else{
            transform.position -= new Vector3(0.0f, 0.2f, 0.0f) * Time.deltaTime;
        }

        if (yPos > 0.5f)
        {
            onWayUp = false;
        }
        else if (yPos <= 0.3f)
        {
            onWayUp = true;
        }
    }
}
