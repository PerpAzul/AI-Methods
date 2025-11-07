using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;     // Stores the movement input from the player
    public float moveSpeed = 5f;   // Speed multiplier for movement
    
    void Update()
    {
        Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        movementDirection.Normalize();
        
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
