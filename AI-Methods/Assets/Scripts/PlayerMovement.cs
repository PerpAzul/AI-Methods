using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump / Gravity")] 
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod; // buffer + coyote window
    [SerializeField] private float groundedStick; // small downward force

    [Header("Ground Check")] 
    [SerializeField] private LayerMask groundLayers; // set to your environment layers only
    [SerializeField] private float groundedRadius;
    [SerializeField] private float groundedOffset; // probe slightly below capsule bottom
    
    private Vector2 moveInput;
    private CharacterController controller;
    private Animator animator;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    // cache animator hashes
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int IsFallingHash = Animator.StringToHash("IsFalling");
    private static readonly int InputMagHash = Animator.StringToHash("Input Magnitude");

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        originalStepOffset = controller.stepOffset;
    }

    void Update()
    {
        // 1) Camera-relative XZ movement
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        float inputMag = Mathf.Clamp01(moveDir.magnitude);
        animator.SetFloat(InputMagHash, inputMag, 0.05f, Time.deltaTime);

        float moveSpeed = inputMag * maximumSpeed;
        if (cameraTransform)
            moveDir = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up) * moveDir;

        moveDir = (moveDir.sqrMagnitude > 0f) ? moveDir.normalized : Vector3.zero;

        // 2) PRE-MOVE ground check (robust) for jump/coyote decisions
        bool wasGrounded = animator.GetBool(IsGroundedHash);
        if (wasGrounded) lastGroundedTime = Time.time;

        float timeSinceGrounded =
            lastGroundedTime.HasValue ? Time.time - lastGroundedTime.Value : float.PositiveInfinity;
        float timeSinceJumpPress = 
            jumpButtonPressedTime.HasValue ? Time.time - jumpButtonPressedTime.Value : float.PositiveInfinity;

        // 3) Jump buffer + coyote time
        bool canJumpNow = (timeSinceJumpPress <= jumpButtonGracePeriod) && (timeSinceGrounded <= jumpButtonGracePeriod);
        
        if (canJumpNow)
        {
            ySpeed = jumpSpeed;
            jumpButtonPressedTime = null; // consume
            lastGroundedTime = null; // avoid double-trigger within same window
            animator.SetBool(IsJumpingHash, true);
            animator.SetBool(IsFallingHash, false);
        }

        // 4) Gravity / stick-to-ground
        if (wasGrounded && ySpeed < 0f) ySpeed = groundedStick;
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // 5) Step offset: allow stepping when grounded or within coyote time
        controller.stepOffset = (wasGrounded || timeSinceGrounded <= jumpButtonGracePeriod) ? originalStepOffset : 0f;

        // 6) Move once
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;
        controller.Move(velocity * Time.deltaTime);

        // 7) POST-MOVE ground check (final state for this frame)
        bool grounded = controller.isGrounded;

        // 8) Animator state machine
        animator.SetBool(IsGroundedHash, grounded);
        if (grounded) lastGroundedTime = Time.time;

        if (grounded)
        {
            // Landed
            if (ySpeed < 0f) ySpeed = groundedStick;
            animator.SetBool(IsFallingHash, false);
            animator.SetBool(IsJumpingHash, false);
        }
        else
        {
            // Airborne
            // Falling if vertical velocity is negative and we’re not in the jump impulse
            bool falling = ySpeed < -0.1f;
            animator.SetBool(IsFallingHash, falling);
            // Keep IsJumping true only for the ascent frames after jump
            if (falling) animator.SetBool(IsJumpingHash, false);
        }

        // 9) Face move direction + walk bool
        if (moveDir != Vector3.zero)
        {
            animator.SetBool(IsMovingHash, true);
            Quaternion toRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool(IsMovingHash, false);
        }
    }

    // New Input System (Player Input → Send Messages)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    } 

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpButtonPressedTime = Time.time;
        }
    }
    

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}