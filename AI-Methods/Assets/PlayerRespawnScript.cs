using UnityEngine;

public class PlayerRespawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("space key was pressed");
            transform.position = new Vector3(0.0f, 0.5f, 0.0f);
        }
    }
}
