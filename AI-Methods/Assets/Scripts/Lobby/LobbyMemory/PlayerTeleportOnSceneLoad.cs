using UnityEngine;
using Unity.Cinemachine;

public class PlayerTeleportOnSceneLoad : MonoBehaviour
{
    public Transform tutorialTeleporter;
    public Transform lobbyTeleporter;
    public GameObject tutorialCanvas;
    public CinemachineOrbitalFollow orbitalFollow;

    void Start()
    {
        GetComponent<CharacterController>().enabled = false;
        if (!VariableStore.IsLobbyTutorialFinished()) {
            transform.position = tutorialTeleporter.position;
            orbitalFollow.HorizontalAxis.Value = 90f;
            tutorialCanvas.SetActive(true);
        } else if (PlayerPositionMemory.lastPosition != Vector3.zero) {
            transform.position = PlayerPositionMemory.lastPosition;
            orbitalFollow.HorizontalAxis.Value = PlayerPositionMemory.lastCameraRotation;
            tutorialCanvas.SetActive(false);
        } else {
            transform.position = lobbyTeleporter.position;
            tutorialCanvas.SetActive(false);
        }
        GetComponent<CharacterController>().enabled = true;
    }
}