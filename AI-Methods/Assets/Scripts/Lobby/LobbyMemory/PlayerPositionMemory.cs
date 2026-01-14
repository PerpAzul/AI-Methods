using UnityEngine;

public class PlayerPositionMemory : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;
    public GameObject player;

    void Awake()
    {
        InvokeRepeating(nameof(UpdateDelayed), 1f, 0.5f);
        DontDestroyOnLoad(gameObject);
    }

    void UpdateDelayed() {
        if (player != null) {
            lastPosition = player.transform.position;
        }
    }
}
