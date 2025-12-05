using UnityEngine;

public enum ActionType
{
    ShowMessageWithE,
    ShowMessageNoE,
    LeadToTarget
}

[System.Serializable]
public class NPCAction
{
    public ActionType type;
    
    [Header("If Type is ShowMessage")]
    [TextArea] public string message; // The text to display
    
    [Header("If Type is LeadToTarget")]
    public Transform targetObject; // Where to walk
}