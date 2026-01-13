using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCGuide : MonoBehaviour
{
    private static readonly int RollAnim = Animator.StringToHash("Walk_Anim");
    
    [Header("The Action List")]
    public List<NPCAction> helpActions;

    [Header("References")]
    public Transform player;        
    public GameObject messageCanvas;
    public TextMeshProUGUI messageTextMesh;
    public TextMeshProUGUI textE;
    public GameObject arrowCanvas;

    [Header("Settings")]
    public float warteAbstand = 4.0f;

    // This is a list of help actions that will be skipped immediately
    private List<string> alreadyDoneActions;


    private int currentActionIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    
    private float playerDistance;
    private bool arrived;
    // This attribute blocks picking up items, items can be picked up when the tutorial is complete and the player
    // it not already picking up an item
    public bool canPickup = false;
    public bool canScanTutorial = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        alreadyDoneActions = new List<string>();
        
        StartCurrentAction();
    }

    void Update()
    {
        if (currentActionIndex >= helpActions.Count) {
            // disable robot when tutorial is done
            messageCanvas.SetActive(false);
            arrowCanvas.SetActive(false);
            return;
        }
        
        playerDistance = Vector3.Distance(transform.position, player.position);
        
        NPCAction currentAction = helpActions[currentActionIndex];
        
        switch (currentAction.type)
        {
            case ActionType.ShowMessageWithE:
            case ActionType.ShowMessageNoE:
                Dialog();
                break;
            case ActionType.LeadToTarget:
                Guide();
                break;
        }
    }

    public void ContinueIfCurrentActionEquals(string actionName)
    {
        alreadyDoneActions.Add(actionName);
        if (currentActionIndex >= helpActions.Count) return;
        if (helpActions[currentActionIndex].actionName == actionName)
        {
            StartNextAction();
        }
    }

    private void StartNextAction()
    {
        currentActionIndex++;
        StartCurrentAction();
    }

    private void StartCurrentAction()
    {
        // Check if we finished all actions
        if (currentActionIndex >= helpActions.Count) return;

        NPCAction currentAction = helpActions[currentActionIndex];
        if (alreadyDoneActions.Contains(currentAction.actionName))
        {
            StartNextAction();
            return;
        }
        arrived = false; // Reset arrival state

        if (currentAction.actionName.Equals("crystal_f") || currentAction.actionName.Equals("tomato_f"))
        {
            canPickup = true;
        }

        if (currentAction.actionName.Equals("scan") || currentAction.actionName.Equals("tomato_scan"))
        {
            canScanTutorial = true;
        }

        if (currentAction.type == ActionType.LeadToTarget)
        {
            agent.ResetPath(); // Clear old path
            messageCanvas.SetActive(false);
            arrowCanvas.SetActive(false);
            
            agent.isStopped = false;
            // Validate target exists to prevent crash
            if(currentAction.targetObject)
                agent.SetDestination(currentAction.targetObject.position);
        }
        else
        {
            // Setup UI
            textE.gameObject.SetActive(currentAction.type == ActionType.ShowMessageWithE);
            messageTextMesh.text = currentAction.message;
            messageCanvas.SetActive(true);
            arrowCanvas.SetActive(false);
            
            agent.isStopped = true;
            animator.SetBool(RollAnim, false);
        }
    }
    
    private void Guide() 
    {
        if (arrived) return;
        
        NPCAction currentAction = helpActions[currentActionIndex];
        if (!currentAction.targetObject)
        {
            StartNextAction();
            return;
        }

        // 1. MOVEMENT LOGIC (Follow/Wait)
        if (playerDistance < warteAbstand)
        {
            arrowCanvas.SetActive(false);
            agent.isStopped = false;
            // Ensure we keep target updated
            agent.SetDestination(currentAction.targetObject.position); 
        }
        else
        {
            agent.isStopped = true; 
            arrowCanvas.SetActive(true);
        }

        // Animation
        var isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool(RollAnim, isMoving);
        
        // 2. ARRIVAL LOGIC (The Fixed Version)
        // Safety: Don't check if we are still calculating path
        if (agent.pathPending) return;

        // Check if NavMesh thinks we are close
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // DOUBLE CHECK: Physical Distance
            // This prevents the "Ghost Arrival" where ResetPath() triggers a skip
            float distToTarget = Vector3.Distance(transform.position, currentAction.targetObject.position);
            
            // Only finish if we are physically close (e.g. 2.0 units)
            if (distToTarget < 2.0f) 
            {
                if (!arrived)
                {
                    arrived = true;
                    StartNextAction();
                }
            }
        }
    }

    private void Dialog()
    {
        // Only show UI if player is close
        if (playerDistance < 2.0f) 
        {
            messageCanvas.SetActive(true);
            arrowCanvas.SetActive(false);

            NPCAction currentAction = helpActions[currentActionIndex];

            if (currentAction.type == ActionType.ShowMessageWithE)
            {
                // Wait for Input
                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartNextAction();   
                }
            }
        }
        else
        {
            // Hide UI if player walks away
            messageCanvas.SetActive(false);
            arrowCanvas.SetActive(true);
        }
    }
}