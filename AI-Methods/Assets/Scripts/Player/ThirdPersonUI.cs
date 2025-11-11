using UnityEngine;
using TMPro;

public class ThirdPersonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
