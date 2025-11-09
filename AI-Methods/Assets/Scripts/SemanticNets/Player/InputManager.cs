using UnityEngine;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public class OnFootActions
    {
        public bool Interact => Input.GetMouseButtonDown(1);
        public bool Destroy => Input.GetMouseButtonDown(0);
    }

    public OnFootActions onFoot = new OnFootActions();

    private PlayerMotor motor;
    private PlayerLook look;

    [Header("Movement Settings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Mouse Settings")]
    public string mouseX = "Mouse X";
    public string mouseY = "Mouse Y";

    [Header("Key Bindings")]
    public KeyCode jumpKey = KeyCode.Space;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
    }

    void FixedUpdate()
    {
        // Bewegung (WASD)
        float x = Input.GetAxis(horizontalAxis);
        float y = Input.GetAxis(verticalAxis);
        Vector2 movement = new Vector2(x, y);

        motor.ProcessMove(movement);
    }

    void LateUpdate()
    {
        // Kamera / Mausbewegung
        float mouseXValue = Input.GetAxis(mouseX);
        float mouseYValue = Input.GetAxis(mouseY);
        Vector2 lookInput = new Vector2(mouseXValue, mouseYValue);

        look.ProcessLook(lookInput);
    }

    void Update()
    {
        // Springen
        if (Input.GetKeyDown(jumpKey))
        {
            motor.Jump();
        }
    }
}

