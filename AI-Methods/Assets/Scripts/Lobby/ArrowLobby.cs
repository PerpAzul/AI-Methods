using UnityEngine;
using System.Collections;

public class ArrowLobby : MonoBehaviour
{
    public InteractableI interactable;
    public Canvas arrowCanvas;
    public float showAfterSeconds = 70f;

    public void Awake() {
        arrowCanvas.enabled = false;
        StartCoroutine(spawnArrowAfterSeconds(showAfterSeconds));
    }

    public void Start() {
        if (VariableStore.IsLobbyTutorialFinished()) {
            this.gameObject.SetActive(false);
        }
    }

    public void Update() {
        if (arrowCanvas.enabled) {
            float newY = interactable.transform.position.y + 1.4f + Mathf.Sin(Time.time * 3f) * 0.2f;
            this.transform.localPosition = new Vector3(interactable.transform.position.x, newY, interactable.transform.position.z);
            SetArrowRotation();
        }
        if (interactable != null && interactable.hasAlreadyInteracted()) {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator spawnArrowAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        this.transform.localPosition = interactable.transform.localPosition + new Vector3(0f, 2f, 0f);
        arrowCanvas.enabled = true;
        SetArrowRotation();
    }

    private void SetArrowRotation() {
        // let the arrow face the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (interactable.transform.position - player.transform.position).normalized;
        this.transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
        Vector3 angles = this.transform.localEulerAngles;
        this.transform.localEulerAngles = new Vector3(0f, angles.y, -90f);
    }
}
