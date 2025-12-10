using UnityEngine;

public class PlayerPositionMemory : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
