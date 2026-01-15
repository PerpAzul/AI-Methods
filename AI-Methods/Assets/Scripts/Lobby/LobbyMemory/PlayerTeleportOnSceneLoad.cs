using UnityEngine;

public class PlayerTeleportOnSceneLoad : MonoBehaviour
{
    public Transform tutorialTeleporter;
    public Transform lobbyTeleporter;
    public GameObject tutorialCanvas;

    void Awake() {
        Debug.Log("Awake called");
    }

    void Start()
    {
        Debug.Log("Start called");
        Debug.Log(VariableStore.IsLobbyTutorialFinished());
        GetComponent<CharacterController>().enabled = false;
        if (!VariableStore.IsLobbyTutorialFinished()) {
            transform.position = tutorialTeleporter.position;
            tutorialCanvas.SetActive(true);
        } else if (PlayerPositionMemory.lastPosition != Vector3.zero) {
            transform.position = PlayerPositionMemory.lastPosition;
            Debug.Log("Set to position: " + transform.position);
            tutorialCanvas.SetActive(false);
        } else {
            transform.position = lobbyTeleporter.position;
            tutorialCanvas.SetActive(false);
        }
        GetComponent<CharacterController>().enabled = true;
    }
}