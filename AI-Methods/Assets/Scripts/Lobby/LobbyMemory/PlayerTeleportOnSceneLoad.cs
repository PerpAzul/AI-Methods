using UnityEngine;

public class PlayerTeleportOnSceneLoad : MonoBehaviour
{
    public Transform tutorialTeleporter;
    public Transform lobbyTeleporter;
    public GameObject tutorialCanvas;

    void Start()
    {
        GetComponent<CharacterController>().enabled = false;
        if (!VariableStore.IsLobbyTutorialFinished()) {
            transform.position = tutorialTeleporter.position;
            tutorialCanvas.SetActive(true);
        } else if (PlayerPositionMemory.lastPosition != Vector3.zero) {
            transform.position = PlayerPositionMemory.lastPosition;
            tutorialCanvas.SetActive(false);
        } else {
            transform.position = lobbyTeleporter.position;
            tutorialCanvas.SetActive(false);
        }
        GetComponent<CharacterController>().enabled = true;
    }
}