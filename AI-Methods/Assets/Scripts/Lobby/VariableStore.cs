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
        PlayerPrefs.Save();
    }

    public static int GetCurrentLevelSemantic()
    {
        while (PlayerPrefs.GetInt("SemanticNetsLevelFinished" + PlayerPrefs.GetInt("SemanticNetsLevel")) == 1) {
            PlayerPrefs.SetInt("SemanticNetsLevel", PlayerPrefs.GetInt("SemanticNetsLevel") + 1);
        }
        return PlayerPrefs.GetInt("SemanticNetsLevel");
    }

    public static void MarkLevelAsFinishedSemantic(int level)
    {
        PlayerPrefs.SetInt("SemanticNetsLevelFinished" + level, 1);
        PlayerPrefs.SetInt("SemanticNetsLevel", level + 1);
        PlayerPrefs.Save();
    }


    // Depth First Search

    public static int GetScoresSearch(int level)
    {
        return PlayerPrefs.GetInt("DepthFirstSearchScore" + level);
    }

    public static void SetScoreSearch(int score, int level)
    {
        PlayerPrefs.SetInt("DepthFirstSearchScore" + level, score);
        PlayerPrefs.Save();
    }

    public static int GetCurrentLevelSearch()
    {
        while (PlayerPrefs.GetInt("DepthFirstSearchLevelFinished" + PlayerPrefs.GetInt("DepthFirstSearchLevel")) == 1) {
            PlayerPrefs.SetInt("DepthFirstSearchLevel", PlayerPrefs.GetInt("DepthFirstSearchLevel") + 1);
        }
        return PlayerPrefs.GetInt("DepthFirstSearchLevel");
    }

    public static void MarkLevelAsFinishedSearch(int level)
    {
        PlayerPrefs.SetInt("DepthFirstSearchLevelFinished" + level, 1);
        PlayerPrefs.SetInt("DepthFirstSearchLevel", level + 1);
        PlayerPrefs.Save();
    } 


    // Decision Trees

    public static int GetScoresDecision(int level)
    {
        return PlayerPrefs.GetInt("DecisionTreesScore" + level);
    }

    public static void SetScoreDecision(int score, int level)
    {
        PlayerPrefs.SetInt("DecisionTreesScore" + level, score);
        PlayerPrefs.Save();
    }

    public static int GetCurrentLevelDecision()
    {
        while (PlayerPrefs.GetInt("DecisionTreesLevelFinished" + PlayerPrefs.GetInt("DecisionTreesLevel")) == 1) {
            PlayerPrefs.SetInt("DecisionTreesLevel", PlayerPrefs.GetInt("DecisionTreesLevel") + 1);
        }
        return PlayerPrefs.GetInt("DecisionTreesLevel");
    }

    public static void MarkLevelAsFinishedDecision(int level)
    {
        PlayerPrefs.SetInt("DecisionTreesLevelFinished" + level, 1);
        PlayerPrefs.SetInt("DecisionTreesLevel", level + 1);
        PlayerPrefs.Save();
    }
}
