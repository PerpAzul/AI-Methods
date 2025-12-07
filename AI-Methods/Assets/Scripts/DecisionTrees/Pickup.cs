using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Pickup: HintDisplayer
{
    [Header("Attributes")] 
    public bool isMetal;
    public bool isDangerous;
    public bool isBlueEnergy;
    
    private bool playerIsClose;
    private bool isPickingUp;
    
    [Header("Player target")]
    public GameObject target;
    
    public RenderTexture texture2d;
    public NPCGuide guide;
    private FindHelper findHelper;
    
    private void Start()
    {
        message = "Drücke [F] zum Aufnehmen";
        findHelper = GameObject.Find("GameManager").GetComponent<FindHelper>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isPickingUp)
        {
            switch(this.name)
            {
                case "crystal_17_2":
                    if (guide) guide.ContinueIfCurrentActionEquals("crystal_f");
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
            this.transform.parent = target.transform;
            this.transform.localEulerAngles = Vector3.zero;
            message = "Drücke [F] zum Ablegen";
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) && isPickingUp)
        {
            isPickingUp = false;
            this.transform.parent = null;
            message = "Drücke [F] zum Aufnehmen";
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        playerIsClose = true;
    }

    void OnTriggerExit(Collider other)
    {
        playerIsClose = false;
    }
}
