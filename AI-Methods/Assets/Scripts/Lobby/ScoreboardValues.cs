using UnityEngine;

public static class ScoreboardValues 
{
    private const string SemanticRecord = "SemanticBestTime";
    private const string SearchRecord = "SearchBestTime";
    private const string PatternRecord = "PatternBestTime";

    // Returns best time in seconds, or +∞ if none saved yet
    public static float SemanticBestTime
    {
        get => PlayerPrefs.GetFloat(SemanticRecord, float.PositiveInfinity);
    }
    
    public static float SearchBestTime
    {
        get => PlayerPrefs.GetFloat(SearchRecord, float.PositiveInfinity);
    }
    
    public static float PatternBestTime
    {
        get => PlayerPrefs.GetFloat(PatternRecord, float.PositiveInfinity);
    }

    // Call this when the level ends
    public static bool SemanticSetBestTime(float newTimeSeconds)
    {
        if (newTimeSeconds < SemanticBestTime)
        {
            PlayerPrefs.SetFloat(SemanticRecord, newTimeSeconds);
            PlayerPrefs.Save();
            return true; // new record
        }

        return false; // didn't beat best time
    }
    
    public static bool SearchSetBestTime(float newTimeSeconds)
    {
        if (newTimeSeconds < SearchBestTime)
        {
            PlayerPrefs.SetFloat(SearchRecord, newTimeSeconds);
            PlayerPrefs.Save();
            return true; // new record
        }

        return false; // didn't beat best time
    }
    
    public static bool PatternSetBestTime(float newTimeSeconds)
    {
        if (newTimeSeconds < PatternBestTime)
        {
            PlayerPrefs.SetFloat(PatternRecord, newTimeSeconds);
            PlayerPrefs.Save();
            return true; // new record
        }

        return false; // didn't beat best time
    }
}
