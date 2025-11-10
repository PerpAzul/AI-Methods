using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraRecenter : MonoBehaviour
{
    [SerializeField] private float recenterTime = 0.8f;
    private CinemachineOrbitalFollow orbital;
    private Transform followTarget;
    private bool recentering;

    void Awake()
    {
        var cam = GetComponent<CinemachineCamera>();
        orbital = GetComponent<CinemachineOrbitalFollow>();
        followTarget = cam.Follow; // this is your player
    } 

    // PlayerInput (Send Messages). Bind action name: "CameraRecenter"
    public void Recenter(InputValue value)
    {
        if (!value.isPressed || orbital == null || followTarget == null || recentering)
            return;

        StartCoroutine(RecenterToBehindTarget());
    }

    private IEnumerator RecenterToBehindTarget()
    {
        recentering = true;

        // Find the yaw angle (horizontal orbit) that looks directly behind the player
        Vector3 targetForward = followTarget.forward;
        float targetYaw = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;

        // Current yaw comes from OrbitalFollow.HorizontalAxis.Value
        var axis = orbital.HorizontalAxis;
        float startYaw = axis.Value;
        float delta = Mathf.DeltaAngle(startYaw, targetYaw);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / recenterTime;
            float newYaw = startYaw + delta * Mathf.SmoothStep(0, 1, t);
            axis.Value = newYaw;
            orbital.HorizontalAxis = axis;
            yield return null;
        }

        recentering = false;
    }
}
