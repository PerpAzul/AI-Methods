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
        if (Input.GetKeyDown(KeyCode.L) && reachedEnd)
        {
            SceneManager.LoadScene(nextScene);
        }

        if ((Input.GetKeyDown("2") || Input.GetKeyDown(KeyCode.Keypad2)) && reachedEnd)
        {
            //Debug.Log("first step");
            if (whichLevel == 1)
            {
                //Debug.Log("got this far");
                SceneManager.LoadScene("02-Tiefensuche");
                //Debug.Log("got even further");
            }
            
        }

        /*if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("pressed A");
        }*/
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
                int geschafftePunkte = 100 - (PlatformRoot.counterWrongPlatforms * 25);
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
