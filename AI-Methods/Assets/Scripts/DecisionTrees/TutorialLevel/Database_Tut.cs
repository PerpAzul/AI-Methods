using System;
using System.Collections.Generic;
using DecisionTrees;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Database_Tut : MonoBehaviour
{
    [Header("Database")] 
    public GameObject databaseCanvas;

    public GameObject empty;
    public GameObject items;
    public TextMeshProUGUI indexText;
    public GameObject preview;
    public Toggle redToggle;
    public Toggle fruitToggle;
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

    public List<Item_Tut> ScannedItems = new();
    public int index;

    [SerializeField]
    // Database nur scrollen wenn player near
    private CanvasToggle playerNearCanvasToggle;
    
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
                $"{index + 1}/4";
            
            if (ScannedItems[index].Useful())
            {
                useful.text = "Guter Gegenstand";
                useful.color = new Color32(34, 255, 0, 255);
            }
            else
            {
                useful.text = "Schlechter Gegenstand";
                useful.color = new Color32(255, 0, 0, 255);
            }

            redToggle.isOn = ScannedItems[index].IsRed;
            fruitToggle.isOn = ScannedItems[index].IsFruit;
            preview.GetComponent<RawImage>().texture = ScannedItems[index].Texture;
        }
        else
        {
            items.SetActive(false);
            empty.gameObject.SetActive(true);
        }
    }

    public bool ContainsPickup(Pickup_Tut pickup)
    {
        foreach (var scannedItem in ScannedItems)
        {
            if (scannedItem.IsRed == pickup.isRed
                && scannedItem.IsFruit == pickup.isFruit)
            {
                return true;
            }
        }

        return false;
    }

    public void DisplayEvaluate(Item_Tut item, float progress)
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

    public void AddNewItem(Item_Tut item)
    {
        // Don't add item twice
        foreach (var scannedItem in ScannedItems)
        {
            if (scannedItem.IsRed == item.IsRed
                && scannedItem.IsFruit == item.IsFruit)
            {
                return;
            }
        }
        ScannedItems.Add(item);
        index = ScannedItems.Count - 1;
        newItemParticles.Play();
        DisplayDatabase();
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (playerNearCanvasToggle.isPlayerNear && databaseCanvas.activeSelf && ScannedItems.Count > 0)
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
}
