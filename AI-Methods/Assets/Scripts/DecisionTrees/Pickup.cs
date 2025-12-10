using UnityEngine;

public class Pickup: HintDisplayer
{
    [Header("Attributes")] 
    public bool isMetal;
    public bool isDangerous;
    public bool isBlueEnergy;
    
    private bool playerIsClose;
    public bool isPickingUp;
    
    [Header("Player target")]
    public GameObject target;
    
    public RenderTexture texture2d;
    public NPCGuide guide;
    private FindHelper findHelper;
    public Database database;
    [SerializeField] private Transform player;

    private void Start()
    {
        message = "Drücke [F] zum Aufnehmen";
        isEnabled = false;
        findHelper = GameObject.Find("GameManager").GetComponent<FindHelper>();
    }
    
    void Update()
    {
        if (guide && guide.canPickupTutorial)
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
            if (guide && !guide.canPickupTutorial)
            {
                return;
            }
            switch(this.name)
            {
                case "crystal_17_2":
                    if (guide) {
                        guide.ContinueIfCurrentActionEquals("crystal_f");
                    }
                    break;
                case "decorative_plant":
                    findHelper.find(2);
                    break;
                case "Crate Short":
                    findHelper.find(3);
                    break;
                case "TrashbinRed":
                    findHelper.find(4);
                    break;
                case "OilDrum":
                    findHelper.find(5);
                    break;
                case "Barrel":
                    findHelper.find(6);
                    break;
                case "FuelTank (2)":
                    findHelper.find(7);
                    break;
                case "Beer Can Blue":
                    findHelper.find(8);
                    break;
                default:
                    break;
            }
            
            isPickingUp = true;
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            this.transform.parent = target.transform;
            message = "Drücke [F] zum Ablegen";
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) && isPickingUp)
        {
            isPickingUp = false;
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
