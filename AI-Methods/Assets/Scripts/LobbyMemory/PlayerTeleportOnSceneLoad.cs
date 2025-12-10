using UnityEngine;

public class PlayerTeleportOnSceneLoad : MonoBehaviour
{
    void Start()
    {
        if (PlayerPositionMemory.lastPosition != Vector3.zero)
        {
            transform.position = PlayerPositionMemory.lastPosition;
        }
    }
}