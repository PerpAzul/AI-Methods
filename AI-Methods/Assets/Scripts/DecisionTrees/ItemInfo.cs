using System;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("UI References")] 
    [SerializeField] private GameObject infoCanvas;
    [SerializeField] private TMP_Text infoText;
    
    [Header("Attributes")] 
    public bool istMetal;
    public bool isDangerous;
    public bool isBlueEnergy;

    [Header("Settings")] [SerializeField] private float floatHeight = 2.0f;

    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        
        GenerateInfoText();
        
        infoCanvas.SetActive(false);
    }

    void GenerateInfoText()
    {
        string metalStr = istMetal ? "✅ Metall" : "❌ Metall";
        string dangerStr = isDangerous ? "✅ Schädlich" : "❌ Schädlich";
        string energyStr = isBlueEnergy ? "✅ Blaue Energie" : "❌ Blaue Energie";

        infoText.text = $"{metalStr} | {dangerStr} |  {energyStr}";
    }

    private void LateUpdate()
    {
        if (infoCanvas.activeSelf)
        {
            infoCanvas.transform.position = transform.position + Vector3.up * floatHeight;
            
            infoCanvas.transform.rotation = Quaternion.LookRotation(infoCanvas.transform.position - mainCamera.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter", other);
        if (other.CompareTag("Player"))
        {
            Debug.Log("player");
            infoCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoCanvas.SetActive(false);
        }
    }
}
