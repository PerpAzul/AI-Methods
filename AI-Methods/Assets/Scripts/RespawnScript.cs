using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        player.transform.position = (new Vector3(0.0f, 0.5f, 0.0f));
        //other.transform etc müsste auch eig funktionieren!!
    }
}
