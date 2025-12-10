using UnityEngine;
using System.Collections;

public class IntroTutorial : MonoBehaviour
{
    [SerializeField] public GameObject target0;   // Dialogue Manager
    [SerializeField] public GameObject target1;   // Essen
    [SerializeField] public GameObject target2;   // Obst
    [SerializeField] public GameObject target3;   // Apfel
    [SerializeField] public GameObject target4;   // unused here (can stay assigned)
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
    // 8: when interacting with second-to-last new target (target5)
    private string[] prompts = new string[]
    {
        // 0 – after dialogue manager
        "Schaue dich um und finde das nächste Ziel.",
        // 1 – after essen
        "Laufe zum nächsten Ziel und stelle eine Verbindung her.",
        // 2 – after obst
        "Pass gut auf! Verbinde „Apfel“ zu „Essen“",
        // 3 – after apfel
        "Pass gut auf! Verbinde „Apfel“ zu „Essen“",
        // 4 – once Apfel–Essen line exists (explanation only)
        "„Apfel“ ist zwar „Essen“, aber am treffendsten ist „Apfel“ als „Obst“ beschrieben.\n Ungenaue Verbindungen bringen keine Punkte!",
        // 5 – separate instruction to delete yellow line
        "Lösche die gelbe, ungenaue Linie mit 'Q'",
        // 6 – once the Apfel–Essen line is deleted
        "Gut gemacht! Bilde jetzt die richtige Verbindung",
        // 7 – once Apfel–Obst line exists
        "Drücke und halte 'T' zum Anzeigen der Minimap.",
        // 8 – when interacting with second-to-last new target
        "Erstelle die letzte Verbindung, um zum nächsten Level zu gelangen."
    };

    // state machine:
    // 0: arrow -> target0, E to interact
    // 2: arrow -> target1, E
    // 4: arrow -> target2, E
    // 6: arrow -> target3, E
    // 8: wait until Apfel–Essen edge exists -> show prompts[4] and [5], then go 9
    // 9: wait until Apfel–Essen edge is deleted -> show prompts[6], then go 10
    // 10: wait until Apfel–Obst edge exists -> show prompts[7] and wait for T -> step 11
    // 11: arrow -> target5 (second-to-last), E shows prompts[8], arrow -> target6, step 13
    // 13: arrow -> target6 (last); E ends tutorial
    // 14+: finished
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
        if (step >= 14)
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
                    step = 8; // now we wait for the Apfel–Essen line
                    StartCoroutine(AfterTargetRoutine(prompts[3], target3));
                }
                break;

            // wait until Apfel–Essen connection exists
            case 8:
                if (HasApfelEssenEdge())
                {
                    step = 9;
                    // show explanation + delete-instruction sequentially
                    StartCoroutine(ApplerEssenCreatedRoutine());
                }
                break;

            // wait until Apfel–Essen connection is deleted
            case 9:
                if (!HasApfelEssenEdge())
                {
                    step = 10;
                    // show "Gut gemacht! Bilde jetzt die richtige Verbindung"
                    StartCoroutine(ShowShortPrompt(prompts[6]));
                }
                break;

            // wait until Apfel–Obst connection exists
            case 10:
                if (HasApfelObstEdge())
                {
                    step = 11;
                    // show minimap hint & wait for T
                    StartCoroutine(MinimapPromptRoutine());
                }
                break;

            // arrow -> second-to-last new target (target5)
            case 11:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    // show "Erstelle die letzte Verbindung..." and move arrow to last target
                    step = 13;
                    StartCoroutine(AfterTargetRoutine(prompts[8], target5));
                }
                break;

            // arrow -> last new target (target6)
            case 13:
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    StopHintIfRunning();
                    // final interaction: end tutorial, arrow disappears and canvas hides
                    interactionCanvas.enabled = false;
                    currentPromptMode = PromptMode.None;
                    step = 14;
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

    // main tutorial prompts: persist until replaced by another tutorial prompt
    private void ShowTutorialPrompt(string text)
    {
        currentPromptMode = PromptMode.Tutorial;
        interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = text;
        interactionCanvas.enabled = true;
    }

    // "Press E" hint: appears after 1s, hidden on trigger exit, doesn't overwrite tutorial prompts
    private IEnumerator ShowHintDelayed(string text)
    {
        currentPromptMode = PromptMode.Hint;
        interactionCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = text;
        yield return new WaitForSeconds(1f);
        interactionCanvas.enabled = true;
    }

    private void StopHintIfRunning()
    {
        if (showCanvasRoutine != null)
        {
            StopCoroutine(showCanvasRoutine);
            showCanvasRoutine = null;
        }
        // don't hide here – tutorial prompt will overwrite the text
    }

    // ---------- Prompt coroutines ----------

    private IEnumerator AfterTargetRoutine(string text, GameObject finishedTarget)
    {
        // wait for dialogue on that target (if any)
        if (finishedTarget != null)
        {
            DialogueManager dm = finishedTarget.GetComponent<DialogueManager>();
            while (dm != null && dm.isInDialogue)
                yield return null;
        }

        // show tutorial prompt and leave it until the next tutorial prompt replaces it
        ShowTutorialPrompt(text);
    }

    // Show prompts[4] then prompts[5] when Apfel–Essen is first created
    private IEnumerator ApplerEssenCreatedRoutine()
    {
        // explanation
        ShowTutorialPrompt(prompts[4]);
        yield return new WaitForSeconds(3f); // just delay before the next prompt

        // delete instruction
        ShowTutorialPrompt(prompts[5]);
        // persists until the player actually deletes the line and we show prompts[6]
    }

    // short one-off tutorial prompt that persists until next one
    private IEnumerator ShowShortPrompt(string text)
    {
        ShowTutorialPrompt(text);
        yield break;
    }

    private IEnumerator MinimapPromptRoutine()
    {
        // after Apfel–Obst is connected
        ShowTutorialPrompt(prompts[7]);

        // wait for T (minimap hint)
        while (!Input.GetKeyDown(KeyCode.T))
            yield return null;

        // do NOT hide the canvas here – prompt stays until the next one (on target5) overwrites it
        // tutorial continues with step 11 (final two targets)
    }

    // ---------- Collider: "Press E" hint ----------

    private bool IsArrowStep(int s) =>
        (s == 0 || s == 2 || s == 4 || s == 6 || s == 11 || s == 13);

    public void OnTriggerEnter(Collider other)
    {
        if (!IsArrowStep(step)) return;

        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

            // only show "Press E" hint if no tutorial prompt is currently active
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

            // Only hide the canvas automatically if we were showing a hint.
            // Tutorial prompts stay visible until replaced by the next one.
            if (currentPromptMode == PromptMode.Hint)
            {
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
            case 11: return target5; // second-to-last new target
            case 13: return target6; // last new target
            default: return null;    // no arrow during edge-waiting / minimap prompt
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
