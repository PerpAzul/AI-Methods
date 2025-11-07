using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float maximumSpeed = 6f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump / Gravity")] [SerializeField]
    private float jumpSpeed = 8f;

    [SerializeField] private float jumpButtonGracePeriod = 0.15f; // buffer + coyote window
    [SerializeField] private float groundedStick = -2f; // small downward force

    [Header("Ground Check")] [SerializeField]
    private LayerMask groundLayers = ~0; // set to your environment layers only

    [SerializeField] private float groundedRadius = 0.25f;
    [SerializeField] private float groundedOffset = 0.05f; // probe slightly below capsule bottom

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

        // Recommended CC settings in Inspector:
        // Min Move Distance = 0, Skin Width ≈ 0.02, Step Offset ≈ 0.3–0.5
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

        moveDir = moveDir.sqrMagnitude > 0f ? moveDir.normalized : Vector3.zero;

        // 2) PRE-MOVE ground check (robust) for jump/coyote decisions
        bool groundedPre = GroundedCheck();
        if (groundedPre) lastGroundedTime = Time.time;

        float timeSinceGrounded =
            lastGroundedTime.HasValue ? Time.time - lastGroundedTime.Value : float.PositiveInfinity;
        float timeSinceJumpPress = jumpButtonPressedTime.HasValue
            ? Time.time - jumpButtonPressedTime.Value
            : float.PositiveInfinity;

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
        if (groundedPre && ySpeed < 0f) ySpeed = groundedStick;
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // 5) Step offset: allow stepping when grounded or within coyote time
        controller.stepOffset = (groundedPre || timeSinceGrounded <= jumpButtonGracePeriod)
            ? originalStepOffset
            : 0f;

        // 6) Move once
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;
        controller.Move(velocity * Time.deltaTime);

        // 7) POST-MOVE ground check (final state for this frame)
        bool groundedPost = GroundedCheck();

        // 8) Animator state machine
        animator.SetBool(IsGroundedHash, groundedPost);

        if (groundedPost)
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
            if (ySpeed <= 0f) animator.SetBool(IsJumpingHash, false);
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
    public void OnMove(InputValue v) => moveInput = v.Get<Vector2>();

    public void OnJump(InputValue v)
    {
        if (v.isPressed) jumpButtonPressedTime = Time.time;
    }

    // Robust grounded probe (independent of CC.isGrounded flicker)
    bool GroundedCheck()
    {
        Vector3 center = transform.position + controller.center;
        float bottomY = center.y - (controller.height * 0.5f) + controller.radius;
        Vector3 probePos = new Vector3(center.x, bottomY - groundedOffset, center.z);
        return Physics.CheckSphere(probePos, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }

}