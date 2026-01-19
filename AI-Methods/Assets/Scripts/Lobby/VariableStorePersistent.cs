using UnityEngine;

public static class VariableStorePersistent
{
    // General

    public static bool IsLobbyTutorialFinished()
    {
        return PlayerPrefs.GetInt("LobbyTutorialFinished") == 1 ? true : false;
    }

    public static void SetLobbyTutorialFinishedStatus(bool finished)
    {
        PlayerPrefs.SetInt("LobbyTutorialFinished", finished ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool IsGameFinished()
    {
        return PlayerPrefs.GetInt("GameFinished") == 1 ? true : false;
    }

    public static void SetGameStateFinished(bool finished)
    {
        PlayerPrefs.SetInt("GameFinished", finished ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void Reset() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public static bool GetFullscreen(){
        return PlayerPrefs.GetInt("Fullscreen", 0) == 1;
    }

    public static void SetFullscreen(bool isFullscreen) {
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static int GetQuality() {
        return PlayerPrefs.GetInt("Quality", 2);
    }

    public static void SetQuality(int qualityIndex) {
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }

    public static int GetResolution() {
        return PlayerPrefs.GetInt("Resolution", 1);
    }

    public static void SetResolution(int resolutionIndex) {
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }

    public static float GetVolume() {
        return PlayerPrefs.GetFloat("Volume", 1f);
    }

    public static void SetVolume(float volumeLevel) {
        PlayerPrefs.SetFloat("Volume", volumeLevel);
        PlayerPrefs.Save();
    }

    public static float GetSensitivity() {
        return PlayerPrefs.GetFloat("Sensitivity", 2f);
    }
    
    public static void SetSensitivity(float sensitivityLevel) {
        PlayerPrefs.SetFloat("Sensitivity", sensitivityLevel);
        PlayerPrefs.Save();
    }


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
