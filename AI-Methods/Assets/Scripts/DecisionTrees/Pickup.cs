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
    
    private void Start()
    {
        message = "Drücke [F] zum Aufnehmen";
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isPickingUp)
        {
            if (this.name.Equals("crystal_17_2"))
            {
                if (guide) guide.ContinueIfCurrentActionEquals("crystal_f");
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
