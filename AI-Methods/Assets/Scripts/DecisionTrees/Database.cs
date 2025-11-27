using System;
using System.Collections.Generic;
using DecisionTrees;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    [Header("Neues Item")] 
    public GameObject newItemCanvas;

    [Header("Database")] 
    public GameObject databaseCanvas;

    public GameObject empty;
    public GameObject items;
    public TextMeshProUGUI indexText;
    public GameObject preview;
    public Toggle metalToggle;
    public Toggle dangerToggle;
    public Toggle enerToggle;
    public TextMeshProUGUI useful;

    [Header("Evaluate")] 
    public GameObject evaluateCanvas;

    public Image progressBar;
    public RawImage evaluateImage;
    public Image evaluateResult;
    public Sprite spriteGreen;
    public Sprite spriteRed;
    public Sprite spriteGray;
    public TextMeshProUGUI evaluateResultText;

    private Item newItem;
    
    public List<Item> ScannedItems = new();
    public int index;

    private bool IsPlayerNear;

    private void Awake()
    {
        databaseCanvas.SetActive(true);
        DisplayDatabase();
    }

    public void DisplayDatabase()
    {
        newItemCanvas.SetActive(false);
        databaseCanvas.SetActive(true);
        if (ScannedItems.Count > 0)
        {
            items.SetActive(true);
            empty.SetActive(false);
            indexText.text =
                $"{index + 1}/{ScannedItems.Count}";
            
            if (ScannedItems[index].Useful())
            {
                useful.text = "Nützlicher Gegenstand";
                useful.color = new Color(34, 255, 0, 255);
            }
            else
            {
                useful.text = "Unnötiger Gegenstand";
                useful.color = new Color(255, 0, 0, 255);
            }

            metalToggle.isOn = ScannedItems[index].IsMetal;
            dangerToggle.isOn = ScannedItems[index].IsDangerous;
            enerToggle.isOn = ScannedItems[index].HasBlueEnergy;
            preview.GetComponent<RawImage>().texture = ScannedItems[index].Texture;
        }
        else
        {
            items.SetActive(false);
            empty.gameObject.SetActive(true);
        }
    }

    public void DisplayEvaluate(Item item, float progress)
    {
        progressBar.fillAmount = progress;
        newItemCanvas.SetActive(false);
        databaseCanvas.SetActive(false);
        evaluateCanvas.SetActive(true);
        evaluateResult.gameObject.SetActive(false);
        
        evaluateImage.texture = item.Texture;
    }

    public void DisplayResult(string result, float progress)
    {
        evaluateResultText.text = result;
        evaluateResult.gameObject.SetActive(true);
        progressBar.fillAmount = progress;
        switch (result)
        {
            case "KORREKT":
                evaluateResult.sprite = spriteGreen;
                break;
            case "FALSCH":
                evaluateResult.sprite = spriteRed;
                break;
            case "?":
                evaluateResult.sprite = spriteGray;
                break;
        }
    }

    public void DisplayNewItem(Item item)
    {
        newItemCanvas.SetActive(true);
        databaseCanvas.SetActive(false);
        evaluateCanvas.SetActive(false);
        newItem = item;
        foreach (Toggle toggle in newItemCanvas.GetComponentsInChildren<Toggle>())
        {
            if (toggle.name == "metal_toggle")
            {
                toggle.isOn = newItem.IsMetal;
            }

            if (toggle.name == "danger_toggle")
            {
                toggle.isOn = newItem.IsDangerous;
            }

            if (toggle.name == "blue_energy_toggle")
            {
                toggle.isOn = newItem.HasBlueEnergy;
            }
        }

        newItemCanvas.GetComponentInChildren<RawImage>().texture = newItem.Texture;
    }

    private void Update()
    {
        if (IsPlayerNear && Input.GetKeyDown(KeyCode.J) && newItemCanvas.activeSelf && newItem.Useful() ||
            IsPlayerNear && Input.GetKeyDown(KeyCode.N) && newItemCanvas.activeSelf && !newItem.Useful())
        {
            Debug.Log($"Adding {newItem}");
            ScannedItems.Add(newItem);
            DisplayDatabase();
        }

        if (IsPlayerNear && databaseCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                index = (index - 1) % ScannedItems.Count;
                DisplayDatabase();
            } else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                index = (index + 1) % ScannedItems.Count;
                DisplayDatabase();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerNear = false;
        }
    }
}
