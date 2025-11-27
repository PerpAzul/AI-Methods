using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Pickup: MonoBehaviour
{
    
    [Header("UI References")] 
    [SerializeField] public GameObject infoCanvas;
    [SerializeField] private TMP_Text infoText;
    
    [Header("Attributes")] 
    public bool isMetal;
    public bool isDangerous;
    public bool isBlueEnergy;
    
    [Header("Settings")] 
    [SerializeField] private Vector3 uiOffset = new(0, 0.6f, 0);
    
    private bool playerIsClose;
    private bool isPickingUp;
    
    [Header("Player target")]
    public GameObject target;
    
    public RenderTexture texture2d;
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        
        infoCanvas.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose && !isPickingUp)
        {
            isPickingUp = true;
            this.transform.parent = target.transform;
            this.transform.localEulerAngles = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) && isPickingUp)
        {
            isPickingUp = false;
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void LateUpdate()
    {
        if (infoCanvas.activeSelf)
        {
            infoCanvas.transform.position = transform.position + uiOffset;
            
            infoCanvas.transform.rotation = Quaternion.LookRotation(infoCanvas.transform.position - mainCamera.transform.position);
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
