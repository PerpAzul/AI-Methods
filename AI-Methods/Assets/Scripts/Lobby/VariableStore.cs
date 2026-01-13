using UnityEngine;

public static class VariableStore
{

    // Semantic Nets

    public static int GetScoresSemantic(int level)
    {
        return PlayerPrefs.GetInt("SemanticNetsScore" + level);
    }

    public static void SetScoreSemantic(int score, int level)
    {
        PlayerPrefs.SetInt("SemanticNetsScore" + level, score);
    }

    public static int GetCurrentLevelSemantic()
    {
        return PlayerPrefs.GetInt("SemanticNetsLevel");
    }

    public static void SetCurrentLevelSemantic(int level)
    {
        PlayerPrefs.SetInt("SemanticNetsLevel", level);
    } 


    // Depth First Search

    public static int GetScoresSearch(int level)
    {
        return PlayerPrefs.GetInt("DepthFirstSearchScore" + level);
    }

    public static void SetScoreSearch(int score, int level)
    {
        PlayerPrefs.SetInt("DepthFirstSearchScore" + level, score);
    }

    public static int GetCurrentLevelSearch()
    {
        return PlayerPrefs.GetInt("DepthFirstSearchLevel");
    }

    public static void SetCurrentLevelSearch(int level)
    {
        PlayerPrefs.SetInt("DepthFirstSearchLevel", level);
    } 


    // Decision Trees

    public static int GetScoresDecision(int level)
    {
        return PlayerPrefs.GetInt("DecisionTreesScore" + level);
    }

    public static void SetScoreDecision(int score, int level)
    {
        PlayerPrefs.SetInt("DecisionTreesScore" + level, score);
    }

    public static int GetCurrentLevelDecision()
    {
        return PlayerPrefs.GetInt("DecisionTreesLevel");
    }

    public static void SetCurrentLevelDecision(int level)
    {
        PlayerPrefs.SetInt("DecisionTreesLevel", level);
    }
}
