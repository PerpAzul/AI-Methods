using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Roadmap : MonoBehaviour
{
    public Sprite levelDue;
    public Sprite levelDone;
    public Image[] levelImagesSemantic;
    public TMP_Text totalScoreTextSemantic;
    public Image[] levelImagesSearch;
    public TMP_Text totalScoreTextSearch;
    public Image[] levelImagesDecision;
    public TMP_Text totalScoreTextDecision;
    
    void Awake()
    {
        int levelSemantic = VariableStore.GetCurrentLevelSemantic();
        int sum = 0;
        for (int i = 0; i < levelImagesSemantic.Length; i++)
        {
            Transform child = null;
            if (levelImagesSemantic[i].transform.childCount > 1) {
                child = levelImagesSemantic[i].transform.GetChild(1);
            }
            if (i < levelSemantic) {
                levelImagesSemantic[i].sprite = levelDone;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = VariableStore.GetScoresSemantic(i) + " Punkte";
                    sum += VariableStore.GetScoresSemantic(i);
                }
            } else {
                levelImagesSemantic[i].sprite = levelDue;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = "0 Punkte";
                }
            }
        }
        totalScoreTextSemantic.text = "Gesamt: " + sum;

        int levelSearch = VariableStore.GetCurrentLevelSearch();
        sum = 0;
        for (int i = 0; i < levelImagesSearch.Length; i++)
        {
            Transform child = null;
            if (levelImagesSearch[i].transform.childCount > 1) {
                child = levelImagesSearch[i].transform.GetChild(1);
            }
            if (i < levelSearch) {
                levelImagesSearch[i].sprite = levelDone;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = VariableStore.GetScoresSearch(i) + " Punkte";
                    sum += VariableStore.GetScoresSearch(i);
                }
            } else {
                levelImagesSearch[i].sprite = levelDue;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = "0 Punkte";
                }
            }
        }
        totalScoreTextSearch.text = "Gesamt: " + sum;

        int levelDecision = VariableStore.GetCurrentLevelDecision();
        sum = 0;
        for (int i = 0; i < levelImagesDecision.Length; i++)
        {
            Transform child = null;
            if (levelImagesDecision[i].transform.childCount > 1) {
                child = levelImagesDecision[i].transform.GetChild(1);
            }
            if (i < levelDecision) {
                levelImagesDecision[i].sprite = levelDone;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = VariableStore.GetScoresDecision(i) + " Punkte";
                    sum += VariableStore.GetScoresDecision(i);
                }
            } else {
                levelImagesDecision[i].sprite = levelDue;
                if (child != null) {
                    child.GetComponent<TMP_Text>().text = "0 Punkte";
                }
            }
        }
        totalScoreTextDecision.text = "Gesamt: " + sum;
    }
}
