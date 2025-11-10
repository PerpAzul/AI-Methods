using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI itemText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateItemText(PlayerInventory playerInventory)
    {
        itemText.text = "Orbs: " + playerInventory.NumberOfItems.ToString();
    }
}
