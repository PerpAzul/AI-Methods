using UnityEngine;

public class PlayerRespawnScript : MonoBehaviour
{

    private Vector3 playerOriginPosition;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerOriginPosition = player.transform.position;
        //transformm = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("space key was pressed");
            //Physics.SyncTransforms();
            player.transform.position = playerOriginPosition;
        }
    }
}
