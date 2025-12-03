using UnityEngine;

public class LookAtMeCanvas : MonoBehaviour
{
    private Canvas myCanvas;
    private Camera myCamera;

    [Header("Rotation Options")]
    public bool onlyRotateY = false;

    private void Awake()
    {
        myCanvas = GetComponent<Canvas>();
        myCamera = Camera.main;
    }

    private void Update()
    {
        if (!onlyRotateY)
        {
            myCanvas.transform.LookAt(myCanvas.transform.position + myCamera.transform.forward);
        }
        else
        {
            Vector3 direction = myCamera.transform.position - myCanvas.transform.position;

            // Remove vertical influence
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                myCanvas.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}