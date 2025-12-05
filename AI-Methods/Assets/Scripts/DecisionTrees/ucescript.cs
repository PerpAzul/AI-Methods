using UnityEngine;

public class ucescript : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private GameObject schinken;
    private GameObject player;
    private bool hasSpawned;

    void Start()
    {
        player = GameObject.Find("Jammo_LowPoly");
    }

    void Update()
    {
        if (!hasSpawned && Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Instantiate(schinken, new Vector3(player.transform.position.x, 1.2f, player.transform.position.z), Quaternion.identity);
            hasSpawned = true;
        }
    }
}
