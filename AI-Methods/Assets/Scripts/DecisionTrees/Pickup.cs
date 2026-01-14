using UnityEngine;

public class Pickup: HintDisplayer
{
    [Header("Attributes")] 
    public bool isMetal;
    public bool isDangerous;
    public bool isBlueEnergy;
    
    private bool playerIsClose;
    public bool isPickingUp;
    public ProgressBar progressBar;
    
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
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();
    }
    
    void Update()
    {
        // only show hint when item not already in database
         if (!isEnabled && !database.ContainsPickup(this))
        {
            isEnabled = true;
        } else if (isEnabled && database.ContainsPickup(this))
        {
            isEnabled = false;
        }

        // only pick up when item not already in database
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isPickingUp && !database.ContainsPickup(this))
        {
            if (this.name.Equals("crystal_17_2")) {
                if (guide) {
                    progressBar.points += 10;
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    guide.ContinueIfCurrentActionEquals("crystal_f");
                }
            }
            
            if (!database.ContainsPickup(this)) {
                if (isBlueEnergy && !isMetal && !isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(2);
                }
                if (!isBlueEnergy && isMetal && !isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(3);
                }
                if (!isBlueEnergy && !isMetal && !isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(4);
                }
                if (!isBlueEnergy && isMetal && isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(5);
                }
                if (!isBlueEnergy && !isMetal && isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(6);
                }
                if (isBlueEnergy && !isMetal && isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(7);
                }
                if (isBlueEnergy && isMetal && !isDangerous)
                {
                    SpawnFloatingText("+10", new Color(0.3f, 1f, 0.3f), this.gameObject.transform.position);
                    progressBar.points += 10;
                    findHelper.find(8);
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

    // from semantic nets
    private void SpawnFloatingText(string text, Color color, Vector3 worldPosition)
    {
        GameObject go = new GameObject("FloatingScoreText");
        go.transform.position = worldPosition;

        var floating = go.AddComponent<FloatingScoreText>();
        floating.SetText(text, color);
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
