using UnityEngine;

public class Pickup_Tut: HintDisplayer
{
    [Header("Attributes")] 
    public bool isRed;
    public bool isFruit;
    
    private bool playerIsClose;
    public bool isPickingUp;
    public ProgressBar_Tut progressBar;
    
    [Header("Player target")]
    public GameObject target;
    
    public RenderTexture texture2d;
    public NPCGuide guide;
    public Database_Tut database;
    [SerializeField] private Transform player;

    private void Start()
    {
        message = "Drücke [F] zum Aufnehmen";
        isEnabled = false;
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar_Tut>();
    }
    
    void Update()
    {
        if (guide && guide.canPickup)
        {
            // only show hint when item not already in database
            if (!isEnabled && !database.ContainsPickup(this))
            {
                isEnabled = true;
            } else if (isEnabled && database.ContainsPickup(this))
            {
                isEnabled = false;
            }
        }

        // only pick up when item not already in database
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isPickingUp && !database.ContainsPickup(this))
        {
            if (guide && !guide.canPickup)
            {
                return;
            }
            if (this.isRed && this.isFruit)
            {
                if (guide) {
                    guide.ContinueIfCurrentActionEquals("tomato_f");
                    guide.ContinueIfCurrentActionEquals("lead_to_tomato");
                }
            }
            if (!this.isRed && this.isFruit) {
                if (guide) {
                    guide.ContinueIfCurrentActionEquals("banana_f");
                    guide.ContinueIfCurrentActionEquals("lead_to_banana");
                }
            }
            if (this.isRed && !this.isFruit) {
                if (guide) {
                    guide.ContinueIfCurrentActionEquals("watermelon_f");
                    guide.ContinueIfCurrentActionEquals("lead_to_watermelon");
                }
            }
            if (!this.isRed && !this.isFruit) {
                if (guide) {
                    guide.ContinueIfCurrentActionEquals("carrot_f");
                    guide.ContinueIfCurrentActionEquals("carrot_found");
                    guide.ContinueIfCurrentActionEquals("lead_to_carrot");
                }
            }
            
            isPickingUp = true;
            if (guide)
            {
                guide.canPickup = false;
            }
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            this.transform.parent = target.transform;
            message = "Drücke [F] zum Ablegen";
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) && isPickingUp)
        {
            isPickingUp = false;
            if (guide)
            {
                guide.canPickup = true;
            }
            message = "Drücke [F] zum Aufnehmen";
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}
