using UnityEngine;
using System.Collections.Generic;

public static class VariableStore
{
    private static bool lobbyTutorialFinished = false;
    private static bool gameFinished = false;
    private static List<int> semanticNetsScores = new List<int>();
    private static List<int> depthFirstSearchScores = new List<int>();
    private static List<int> decisionTreesScores = new List<int>();
    private static List<bool> semanticNetsLevelsFinished = new List<bool>();
    private static List<bool> depthFirstSearchLevelsFinished = new List<bool>();
    private static List<bool> decisionTreesLevelsFinished = new List<bool>();
    private static int currentSemanticNetsLevel = 0;
    private static int currentDepthFirstSearchLevel = 0;
    private static int currentDecisionTreesLevel = 0;

    private static bool fullscreen = false;
    private static int quality = 2;
    private static int resolution = 1;
    private static float volume = 1f;
    private static float sensitivity = 2f;

    // General

    public static bool IsLobbyTutorialFinished()
    {
        return lobbyTutorialFinished;
    }

    public static void SetLobbyTutorialFinishedStatus(bool finished)
    {
        lobbyTutorialFinished = finished;
    }

    public static bool IsGameFinished()
    {
        return gameFinished;
    }

    public static void SetGameStateFinished(bool finished)
    {
        gameFinished = finished;
    }

    public static void Reset() {
        lobbyTutorialFinished = false;
        gameFinished = false;
        semanticNetsScores.Clear();
        depthFirstSearchScores.Clear();
        decisionTreesScores.Clear();
        semanticNetsLevelsFinished.Clear();
        depthFirstSearchLevelsFinished.Clear();
        decisionTreesLevelsFinished.Clear();
        currentSemanticNetsLevel = 0;
        currentDepthFirstSearchLevel = 0;
        currentDecisionTreesLevel = 0;
    }

    public static bool GetFullscreen(){
        return fullscreen;
    }

    public static void SetFullscreen(bool isFullscreen) {
        fullscreen = isFullscreen;
    }

    public static int GetQuality() {
        return quality;
    }

    public static void SetQuality(int qualityIndex) {
        quality = qualityIndex;
    }

    public static int GetResolution() {
        return resolution;
    }

    public static void SetResolution(int resolutionIndex) {
        resolution = resolutionIndex;
    }

    public static float GetVolume() {
        return volume;
    }

    public static void SetVolume(float volumeLevel) {
        volume = volumeLevel;
    }

    public static float GetSensitivity() {
        return sensitivity;
    }
    
    public static void SetSensitivity(float sensitivityLevel) {
        sensitivity = sensitivityLevel;
    }




    // Semantic Nets

    public static int GetScoresSemantic(int level)
    {
        if (semanticNetsScores.Count <= level) {
            return 0;
        }
        return semanticNetsScores[level];
    }

    public static void SetScoreSemantic(int score, int level)
    {
        while (semanticNetsScores.Count <= level) {
            semanticNetsScores.Add(0);
        }
        semanticNetsScores[level] = score;
    }

    public static int GetCurrentLevelSemantic()
    {
        while (semanticNetsLevelsFinished.Count > currentSemanticNetsLevel && semanticNetsLevelsFinished[currentSemanticNetsLevel]) {
            currentSemanticNetsLevel++;
        }
        return currentSemanticNetsLevel;
    }

    public static void MarkLevelAsFinishedSemantic(int level)
    {
        while (semanticNetsLevelsFinished.Count <= level) {
            semanticNetsLevelsFinished.Add(false);
        }
        semanticNetsLevelsFinished[level] = true;
        if (currentSemanticNetsLevel == level) {
            currentSemanticNetsLevel++;
        }
    }


    // Depth First Search

    public static int GetScoresSearch(int level)
    {
        if (depthFirstSearchScores.Count <= level) {
            return 0;
        }
        return depthFirstSearchScores[level];
    }

    public static void SetScoreSearch(int score, int level)
    {
        while (depthFirstSearchScores.Count <= level) {
            depthFirstSearchScores.Add(0);
        }
        depthFirstSearchScores[level] = score;
    }

    public static int GetCurrentLevelSearch()
    {
        while (depthFirstSearchLevelsFinished.Count > currentDepthFirstSearchLevel && depthFirstSearchLevelsFinished[currentDepthFirstSearchLevel]) {
            currentDepthFirstSearchLevel++;
        }
        return currentDepthFirstSearchLevel;
    }

    public static void MarkLevelAsFinishedSearch(int level)
    {
        while (depthFirstSearchLevelsFinished.Count <= level) {
            depthFirstSearchLevelsFinished.Add(false);
        }
        depthFirstSearchLevelsFinished[level] = true;
        if (currentDepthFirstSearchLevel == level) {
            currentDepthFirstSearchLevel++;
        }
    } 


    // Decision Trees

    public static int GetScoresDecision(int level)
    {
        if (decisionTreesScores.Count <= level) {
            return 0;
        }
        return decisionTreesScores[level];
    }

    public static void SetScoreDecision(int score, int level)
    {
        while (decisionTreesScores.Count <= level) {
            decisionTreesScores.Add(0);
        }
        decisionTreesScores[level] = score;
    }

    public static int GetCurrentLevelDecision()
    {
        while (decisionTreesLevelsFinished.Count > currentDecisionTreesLevel && decisionTreesLevelsFinished[currentDecisionTreesLevel]) {
            currentDecisionTreesLevel++;
        }
        return currentDecisionTreesLevel;
    }

    public static void MarkLevelAsFinishedDecision(int level)
    {
        while (decisionTreesLevelsFinished.Count <= level) {
            decisionTreesLevelsFinished.Add(false);
        }
        decisionTreesLevelsFinished[level] = true;
        if (currentDecisionTreesLevel == level) {
            currentDecisionTreesLevel++;
        }
    }
}
