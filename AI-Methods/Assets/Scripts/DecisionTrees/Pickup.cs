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
            if (this.name.Equals("crystal_17_2")) {
                if (guide) {
                    guide.ContinueIfCurrentActionEquals("crystal_f");
                }
            }
            
            if (!database.ContainsPickup(this)) {
                if (isBlueEnergy && !isMetal && !isDangerous)
                {
                    findHelper.find(2);
                }
                if (!isBlueEnergy && isMetal && !isDangerous)
                {
                    findHelper.find(3);
                }
                if (!isBlueEnergy && !isMetal && !isDangerous)
                {
                    findHelper.find(4);
                }
                if (!isBlueEnergy && isMetal && isDangerous)
                {
                    findHelper.find(5);
                }
                if (!isBlueEnergy && !isMetal && isDangerous)
                {
                    findHelper.find(6);
                }
                if (isBlueEnergy && !isMetal && isDangerous)
                {
                    findHelper.find(7);
                }
                if (isBlueEnergy && isMetal && !isDangerous)
                {
                    findHelper.find(8);
                }
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
