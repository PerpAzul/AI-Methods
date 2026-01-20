using UnityEngine;
using Unity.Cinemachine;

public class PlayerPositionMemory : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;
    public static float lastCameraRotation = 0f;
    public GameObject player;
    public CinemachineOrbitalFollow orbitalFollow;

    void Awake()
    {
        InvokeRepeating(nameof(UpdateDelayed), 1f, 0.3f);
        DontDestroyOnLoad(gameObject);
    }

    void UpdateDelayed() {
        if (player != null) {
            lastPosition = player.transform.position;
        }
        if (orbitalFollow != null) {
            lastCameraRotation = orbitalFollow.HorizontalAxis.Value;
        }
    }
}
