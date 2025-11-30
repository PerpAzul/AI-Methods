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
    private string[] prompts = new string[] {
        "Drücke 'E' zum Interagieren.",
        "Drücke 'E' zum Interagieren.",
        "Drücke 'E' zum Interagieren.",
        "Drücke und halte 'T' zum Anzeigen der Minimap."
    };
    private int idx = 0;
    private Coroutine showCanvasRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = new GameObject[] { target0, target1, target2 };
        interactionCanvas.enabled = false;
        interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = prompts[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (idx >= targets.Length) {
            return; // all interactions done
        }
        if (targets[idx].GetComponent<InteractableI>().hasAlreadyInteracted()) {
            interactionCanvas.enabled = false; // hide as soon as interacted
            idx++; // move to next target
            interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = prompts[idx]; // update prompt before checking idx because promts array is longer
            if (idx >= targets.Length) {
                this.GetComponent<Canvas>().enabled = false; // hide arrow without deactivating script
                StartCoroutine(ShowCanvasDelayed()); // show final prompt after delay
                StartCoroutine(WaitForT());
                return;
            }

            // let the arrow face the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 dir = (targets[idx].transform.position - player.transform.position).normalized;
            this.transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
            Vector3 angles = this.transform.localEulerAngles;
            this.transform.localEulerAngles = new Vector3(0f, angles.y, -90f);
        }
        
        // floating arrow animation
        float newY = targets[idx].transform.position.y + 1.15f + Mathf.Sin(Time.time * 3f) * 0.3f;
        this.transform.localPosition = new Vector3(targets[idx].transform.position.x, newY, targets[idx].transform.position.z);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            showCanvasRoutine = StartCoroutine(ShowCanvasDelayed());
        }
    }

    public void OnTriggerExit(Collider other) {
        if (idx >= targets.Length) {
            return; // prevent hiding final prompt
        }
        if (other.gameObject.CompareTag("Player")) {
            if (showCanvasRoutine != null) {
                StopCoroutine(showCanvasRoutine);
            }
            interactionCanvas.enabled = false;
        }
    }

    private IEnumerator ShowCanvasDelayed()
    {
        yield return new WaitForSeconds(1f); // 1 second wait
        interactionCanvas.enabled = true;
    }

    private IEnumerator WaitForT()
    {
        while (!Input.GetKeyDown(KeyCode.T))
        {
            yield return null;
        }
        interactionCanvas.enabled = false;
        this.gameObject.SetActive(false); // deactivate arrow object entirely
    }
}
