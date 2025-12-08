using System;
using System.Collections.Generic;
using DecisionTrees;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
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

    public ParticleSystem newItemParticles;

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
                useful.color = new Color32(34, 255, 0, 255);
            }
            else
            {
                useful.text = "Unnötiger Gegenstand";
                useful.color = new Color32(255, 0, 0, 255);
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

    public void AddNewItem(Item item)
    {
        ScannedItems.Add(item);
        index = ScannedItems.Count - 1;
        newItemParticles.Play();
        DisplayDatabase();
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (IsPlayerNear && databaseCanvas.activeSelf && ScannedItems.Count > 0)
        {
            if (scrollInput > 0)
            {
                index = (index - 1 + ScannedItems.Count) % ScannedItems.Count;
                DisplayDatabase();
            } else if (scrollInput < 0)
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
