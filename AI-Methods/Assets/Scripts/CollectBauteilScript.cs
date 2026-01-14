using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectBauteilScript : MonoBehaviour
{

    public ChangeGlowColorScript bauteilPlatformScript;
    public GameObject geschafftScreen;
    //bool completedAllPlatforms;
    [SerializeField] private string nextScene;
    bool reachedEnd;
    public int whichLevel;
    public TextMeshProUGUI text;
    public RootPlatformScript PlatformRoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //completedAllPlatforms = false;
        geschafftScreen.SetActive(false);
        reachedEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedEnd && Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bauteilPlatformScript.nextPlatform)
        {
            //Debug.Log("found Bauteil!!");
            geschafftScreen.SetActive(true);
            reachedEnd = true;
            if (whichLevel > 0)
            {
                int geschafftePunkte = 300 - (PlatformRoot.counterWrongPlatforms * 50);
                //Debug.Log(geschafftePunkte);
                text.text = "Punkte: " + geschafftePunkte;
                VariableStore.SetScoreSearch(geschafftePunkte, whichLevel);
            }
            VariableStore.MarkLevelAsFinishedSearch(whichLevel);
            //Cursor.lockState = CursorLockMode.None;
            //put "win screen"
        }
        
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
