using UnityEngine;
using System.Collections;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField]
    public GameObject target0;
    [SerializeField]
    public GameObject target1;
    [SerializeField]
    public GameObject target2;
    [SerializeField]
    public Canvas interactionCanvas;

    private GameObject[] targets;
    private string[] prompts = new string[] { // exactly 1 prompt after each target interaction
        "Schaue dich um und finde das nächste Ziel.",
        "Laufe zum nächsten Ziel und stelle eine Verbindung her.",
        "Jetzt kann die KI eine logische Verbindung herstellen.\nDrücke und halte 'T' zum Anzeigen der Minimap."
    };
    private int idxTarget = 0;
    private Coroutine showCanvasRoutine;
    private bool lastInteractionActive = false;
    private bool[] hasAlreadyInteracted = new bool[] { false, false, false };
    private bool playerInRange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = new GameObject[] { target0, target1, target2 };
        SetArrowRotation(); // set initial arrow rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (idxTarget >= targets.Length) {
            return; // all interactions done
        }

        // check if player already interacted with that target
        if (targets[idxTarget].GetComponent<InteractableI>() != null) {
            hasAlreadyInteracted[idxTarget] = targets[idxTarget].GetComponent<InteractableI>().hasAlreadyInteracted();
        } else if (playerInRange && Input.GetKeyDown(KeyCode.E)) {
            hasAlreadyInteracted[idxTarget] = true; // player interacted with target
        }
        if (hasAlreadyInteracted[idxTarget]) {
            interactionCanvas.enabled = false; // hide as soon as interacted
            idxTarget++; // move to next target
            StartCoroutine(showTutorialPrompt()); // show prompt between two targets

            if (idxTarget >= targets.Length) {
                this.GetComponent<Canvas>().enabled = false; // hide arrow without deactivating script
                return;
            }
            
            SetArrowRotation(); // update arrow position to next target
        }
        
        // floating arrow animation
        float newY = targets[idxTarget].transform.position.y + 1.15f + Mathf.Sin(Time.time * 3f) * 0.3f;
        this.transform.localPosition = new Vector3(targets[idxTarget].transform.position.x, newY, targets[idxTarget].transform.position.z);
    }

    private IEnumerator showTutorialPrompt()
    {
        while (targets[idxTarget - 1].GetComponent<DialogueManager>() != null 
                && targets[idxTarget - 1].GetComponent<DialogueManager>().isInDialogue) {
            yield return null;
        }
        yield return ShowCanvasDelayed(prompts[idxTarget - 1]);
        if (idxTarget <= 2) {
            yield return WaitForW();
        } else { // end tutorial
            lastInteractionActive = true;
            yield return WaitForT();
            this.gameObject.SetActive(false); // deactivate arrow object entirely
        }
    }

    // logic for Entering and Exiting arrow collider
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !lastInteractionActive) {
            playerInRange = true;
            showCanvasRoutine = StartCoroutine(ShowCanvasDelayed("Drücke 'E' zum Interagieren"));
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") && !lastInteractionActive) {
            playerInRange = false;
            if (showCanvasRoutine != null) {
                StopCoroutine(showCanvasRoutine);
            }
            interactionCanvas.enabled = false;
        }
    }



    private IEnumerator ShowCanvasDelayed(string text)
    {
        interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = text; // update prompt before checking idx because promts array is longer
        yield return new WaitForSeconds(1f); // 1 second wait
        interactionCanvas.enabled = true;
    }

    private IEnumerator WaitForW()
    {
        while (!Input.GetKeyDown(KeyCode.W))
        {
            yield return null;
        }
        interactionCanvas.enabled = false;
    }

    private IEnumerator WaitForT()
    {
        while (!Input.GetKeyDown(KeyCode.T))
        {
            yield return null;
        }
        interactionCanvas.enabled = false;
    }

    private void SetArrowRotation() {
        // let the arrow face the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (targets[idxTarget].transform.position - player.transform.position).normalized;
        this.transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
        Vector3 angles = this.transform.localEulerAngles;
        this.transform.localEulerAngles = new Vector3(0f, angles.y, -90f);
    }
}
