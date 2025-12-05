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

    [Header("Einstellungen")]
    public float warteAbstand = 4.0f;

    private int currentActionIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    
    private float playerDistance;
    private bool arrived;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        
        StartCurrentAction();
    }

    void Update()
    {
        if (currentActionIndex >= helpActions.Count) return;
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

    private void StartNextAction()
    {
        Debug.Log("starteda;");
        currentActionIndex++;
        StartCurrentAction();
    }

    private void StartCurrentAction()
    {
        NPCAction currentAction = helpActions[currentActionIndex];
        if (currentAction.type == ActionType.LeadToTarget)
        {
            agent.ResetPath();
            messageCanvas.SetActive(false);
            arrowCanvas.SetActive(false);
            agent.isStopped = false;
            agent.SetDestination(currentAction.targetObject.position);
            arrived = false;
        }
        else
        {
            textE.gameObject.SetActive(currentAction.type == ActionType.ShowMessageWithE);
            messageTextMesh.text = currentAction.message;
            messageCanvas.SetActive(true);
            
            agent.isStopped = true;
            animator.SetBool(RollAnim, false);
        }
    }
    
    private void Guide() {
        if (arrived)
        {
            return;
        }
        if (playerDistance < warteAbstand)
        {
            arrowCanvas.SetActive(false);
            agent.isStopped = false;
            agent.SetDestination(helpActions[currentActionIndex].targetObject.position); 
        }
        else
        {
            agent.isStopped = true; 
            arrowCanvas.SetActive(true);
        }

        var isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool(RollAnim, isMoving);
        
        if (!agent.isStopped && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f && !arrived)
            {
                arrived = true;
                StartNextAction(); // We arrived!
            }
        }
    }

    private void Dialog()
    {
        if (playerDistance < 2)
        {
            messageCanvas.SetActive(true);
            arrowCanvas.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E) && helpActions[currentActionIndex].type == ActionType.ShowMessageWithE)
            {
                StartNextAction();   
            }
        }
        else
        {
            messageCanvas.SetActive(false);
            arrowCanvas.SetActive(true);
        }
    }
}