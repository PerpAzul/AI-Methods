using UnityEngine;
using System.Collections;

public class IntroTutorial : MonoBehaviour
{
    [SerializeField] public GameObject target0;   // Dialogue Manager
    [SerializeField] public GameObject target1;   // Essen
    [SerializeField] public GameObject target2;   // Obst
    [SerializeField] public GameObject target3;   // Apfel
    [SerializeField] public GameObject target4;   // Essen (arrow target after Apfel)
    [SerializeField] public GameObject target5;   // second-to-last tutorial target
    [SerializeField] public GameObject target6;   // last tutorial target
    [SerializeField] public Canvas interactionCanvas;

    // Prompts in order:
    // 0: after target0
    // 1: after target1
    // 2: after target2
    // 3: after target3
    // 4: once Apfel–Essen line is created (explanation)
    // 5: immediately after 4: instruction to delete yellow line
    // 6: once that line is deleted
    // 7: once Apfel–Obst is created (minimap)
    // 8: final instruction about last connection
    private string[] prompts = new string[]
    {
        "Schaue dich um und finde das nächste Ziel.",
        "Laufe zum nächsten Ziel und stelle eine Verbindung her.",
        "Pass gut auf! Verbinde „Apfel“ zu „Essen“",
        "Pass gut auf! Verbinde „Apfel“ zu „Essen“",
        "„Apfel“ ist zwar „Essen“, aber am treffendsten ist „Apfel“ als „Obst“ beschrieben.\n Ungenaue Verbindungen bringen keine Punkte!",
        "Lösche die gelbe, ungenaue Linie mit 'Q'",
        "Gut gemacht! Bilde jetzt die richtige Verbindung",
        "Drücke und halte 'T' zum Anzeigen der Minimap.",
        "Erstelle die letzte Verbindung, um zum nächsten Level zu gelangen."
    };

    // state machine:
    //  0: arrow -> target0, E to interact
    //  2: arrow -> target1, E
    //  4: arrow -> target2, E
    //  6: arrow -> target3, E
    //  8: arrow -> target4, wait until Apfel–Essen exists -> show prompts[4] & [5], then go 9
    //  9: wait until Apfel–Essen is deleted -> show prompts[6], then go 10
    // 10: arrow -> target3 (Apfel); E moves arrow to Obst -> step 11
    // 11: arrow -> target2 (Obst), wait until Apfel–Obst exists -> start minimap coroutine, step 12
    // 12: minimap prompt active, waiting for T (no arrow)
    // 13: after T: arrow -> target5, prompt[8] shown
    // 15: arrow -> target6, E ends tutorial
    // 16+: finished
    private int step = 0;

    private bool playerInRange = false;
    private Coroutine showCanvasRoutine;  // only for "Press E" hint
    private Canvas arrowCanvas;

    private enum PromptMode { None, Hint, Tutorial }
    private PromptMode currentPromptMode = PromptMode.None;

    void Start()
    {
        arrowCanvas = GetComponent<Canvas>();

        if (interactionCanvas != null)
            interactionCanvas.enabled = false;

        if (arrowCanvas != null)
            arrowCanvas.enabled = true;
    }

    void Update()
    {
        if (step >= 16)
            return;

        // ---------- Arrow position / rotation ----------
        GameObject arrowTarget = GetArrowTargetForStep(step);
        if (arrowTarget != null)
        {
            if (arrowCanvas != null)
                arrowCanvas.enabled = true;

            float newY = arrowTarget.transform.position.y + 1.15f + Mathf.Sin(Time.time * 3f) * 0.3f;
            transform.localPosition = new Vector3(
                arrowTarget.transform.position.x,
                newY,
                arrowTarget.transform.position.z
            );

            SetArrowRotationTowards(arrowTarget.transform.position);
        }
        else
        {
            if (arrowCanvas != null)
                arrowCanvas.enabled = false;
        }

        // ---------- Per-step logic ----------
        switch (step)
        {
            // target0
            case 0:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    step = 2; // immediately advance arrow to next target
                    StartCoroutine(AfterTargetRoutine(prompts[0], target0));
                }
                break;

            // target1 (Essen)
            case 2:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    step = 4;
                    StartCoroutine(AfterTargetRoutine(prompts[1], target1));
                }
                break;

            // target2 (Obst)
            case 4:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    step = 6;
                    StartCoroutine(AfterTargetRoutine(prompts[2], target2));
                }
                break;

            // target3 (Apfel)
            case 6:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    step = 8; // now we wait for the Apfel–Essen line (arrow goes to target4)
                    StartCoroutine(AfterTargetRoutine(prompts[3], target3));
                }
                break;

            // wait until Apfel–Essen connection exists (arrow on target4)
            case 8:
                if (HasApfelEssenEdge())
                {
                    step = 9;
                    StartCoroutine(ApplerEssenCreatedRoutine());
                }
                break;

            // wait until Apfel–Essen connection is deleted
            case 9:
                if (!HasApfelEssenEdge())
                {
                    step = 10;
                    StartCoroutine(ShowShortPrompt(prompts[6]));
                }
                break;

            // arrow -> Apfel, require E before moving arrow to Obst
            case 10:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    step = 11; // arrow will move to Obst
                }
                break;

            // arrow -> Obst, wait for Apfel–Obst connection
            case 11:
                if (HasApfelObstEdge())
                {
                    // start minimap prompt, hide arrow while waiting for T
                    step = 12;
                    StartCoroutine(MinimapPromptRoutine());
                }
                break;

            // step 12: handled entirely by MinimapPromptRoutine (no arrow here)
            case 12:
                // nothing in Update; coroutine will set step = 13 after T
                break;

            // arrow -> second-to-last new target (target5)
            case 13:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    // keep current prompt (prompts[8]), just move arrow to last target
                    step = 15;
                }
                break;

            // arrow -> last new target (target6)
            case 15:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    if (interactionCanvas != null)
                        interactionCanvas.enabled = false;
                    currentPromptMode = PromptMode.None;
                    step = 16;
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    // ---------- Edge helpers (read LevelManager only) ----------

    private bool HasEdge(GameObject a, GameObject b)
    {
        if (LevelManager.Instance == null || a == null || b == null)
            return false;

        // isValidConnection returns FALSE when the edge already exists
        return !LevelManager.Instance.isValidConnection(a.transform, b.transform);
    }

    private bool HasApfelEssenEdge()
    {
        // Essen (target1) – Apfel (target3)
        return HasEdge(target1, target3);
    }

    private bool HasApfelObstEdge()
    {
        // Apfel (target3) – Obst (target2)
        return HasEdge(target3, target2);
    }

    // ---------- Prompt helpers ----------

    private void ShowTutorialPrompt(string text)
    {
        currentPromptMode = PromptMode.Tutorial;
        if (interactionCanvas != null) {
            interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = text;
            interactionCanvas.enabled = true;
        }
    }

    private IEnumerator ShowHintDelayed(string text)
    {
        currentPromptMode = PromptMode.Hint;
        if (interactionCanvas != null)
            interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = text;
        yield return new WaitForSeconds(1f);
        if (interactionCanvas != null)
            interactionCanvas.enabled = true;
    }

    private void StopHintIfRunning()
    {
        if (showCanvasRoutine != null)
        {
            StopCoroutine(showCanvasRoutine);
            showCanvasRoutine = null;
        }
    }

    // ---------- Prompt coroutines ----------

    private IEnumerator AfterTargetRoutine(string text, GameObject finishedTarget)
    {
        if (finishedTarget != null)
        {
            DialogueManager dm = finishedTarget.GetComponent<DialogueManager>();
            while (dm != null && dm.isInDialogue)
                yield return null;
        }

        ShowTutorialPrompt(text);
    }

private IEnumerator ApplerEssenCreatedRoutine()
{
    // explanation + continue hint
    ShowTutorialPrompt(
        prompts[4] + "\n\n<size=70%>(1/2) Weiterlesen mit 'R'</size>"
    );

    // wait until player presses R
    while (!Input.GetKeyDown(KeyCode.R))
        yield return null;

    // now show delete instruction
    ShowTutorialPrompt(prompts[5]);
    // stays until the player deletes the line and prompt[6] overwrites it
}

    private IEnumerator ShowShortPrompt(string text)
    {
        ShowTutorialPrompt(text);
        yield break;
    }

    // minimap prompt: after Apfel–Obst is connected
    private IEnumerator MinimapPromptRoutine()
    {
        // step is 12 while this runs, so no arrow is shown
        ShowTutorialPrompt(prompts[7]);

        while (!Input.GetKeyDown(KeyCode.T))
            yield return null;

        // After T: advance prompt AND then arrow will appear on target5 (step 13)
        ShowTutorialPrompt(prompts[8]);
        step = 13;
    }

    // ---------- Collider: "Press E" hint ----------

    private bool IsArrowStep(int s) =>
        (s == 0 || s == 2 || s == 4 || s == 6 || s == 10 || s == 11 || s == 13 || s == 15);

    public void OnTriggerEnter(Collider other)
    {
        if (!IsArrowStep(step)) return;

        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

            if (currentPromptMode != PromptMode.Tutorial)
            {
                showCanvasRoutine = StartCoroutine(ShowHintDelayed("Drücke 'E' zum Interagieren"));
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!IsArrowStep(step)) return;

        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;

            if (showCanvasRoutine != null)
            {
                StopCoroutine(showCanvasRoutine);
                showCanvasRoutine = null;
            }

            if (currentPromptMode == PromptMode.Hint)
            {
                if (interactionCanvas != null)
                    interactionCanvas.enabled = false;
                currentPromptMode = PromptMode.None;
            }
        }
    }

    // ---------- Arrow rotation ----------

    private GameObject GetArrowTargetForStep(int s)
    {
        switch (s)
        {
            case 0:  return target0;
            case 2:  return target1;
            case 4:  return target2;
            case 6:  return target3;
            case 8:  return target4; // waiting for Apfel–Essen
            case 10: return target3; // after deletion, arrow back to Apfel
            case 11: return target2; // after interacting with Apfel, arrow to Obst
            case 13: return target5; // AFTER T, arrow to second-to-last new target
            case 15: return target6; // last new target
            default: return null;
        }
    }

    private void SetArrowRotationTowards(Vector3 targetPos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 dir = (targetPos - player.transform.position).normalized;
        transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
        Vector3 angles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(0f, angles.y, -90f);
    }
}
