using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootPlatformScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool PlatformAnswer;
    public int whichPlatform;

    public ChangeGlowColorScript PlatformOne;
    public ChangeGlowColorScript PlatformTwo;
    public ChangeGlowColorScript PlatformThree;
    public ChangeGlowColorScript PlatformFour;
    public ChangeGlowColorScript PlatformFive;
    public ChangeGlowColorScript PlatformSix;
    public ChangeGlowColorScript PlatformSeven;
    /*public ChangeGlowColorScript PlatformEight;
    public ChangeGlowColorScript PlatformNine;
    public ChangeGlowColorScript PlatformTen;*/
    private int current;
    public int counterWrongPlatforms;

    public GameObject infopointOne;
    public GameObject infopointTwo;
    public GameObject infopointThree;


    void Start()
    {
        PlatformAnswer = false;
        PlatformOne.nextPlatform = true;
        PlatformTwo.nextPlatform = false;
        PlatformThree.nextPlatform = false;
        PlatformFour.nextPlatform = false;
        PlatformFive.nextPlatform = false;
        PlatformSix.nextPlatform = false;
        PlatformSeven.nextPlatform = false;
        counterWrongPlatforms = 0;
        infopointOne.SetActive(true);
        infopointTwo.SetActive(false);
        infopointThree.SetActive(false);
    }

    public void ResetSceneFail()
    {
        foreach (var changeGlowColorScript in new List<ChangeGlowColorScript>
                     { PlatformOne, PlatformTwo, PlatformThree, PlatformFour, PlatformFive, PlatformSix, PlatformSeven })
        {
            changeGlowColorScript.ResetPlatform();
        }

        PlatformOne.nextPlatform = true;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (counterWrongPlatforms >= 3)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (PlatformAnswer)
        { 
            PlatformAnswer = false;
            //n�chstes in PlatformReihenfolge auf true setzen
            /*if (PlatformNine.nextPlatform)
            {
                PlatformTen.nextPlatform = true;
            }
            else if (PlatformEight.nextPlatform)
            {
                PlatformNine.nextPlatform = true;
            }
            else if (PlatformSeven.nextPlatform)
            {
                PlatformEight.nextPlatform = true;
            }*/
            if (PlatformSix.nextPlatform && whichPlatform == 6)
            {
                //Debug.Log("next6");
                PlatformSeven.nextPlatform = true;
            }
            else if (PlatformFive.nextPlatform && whichPlatform == 5) 
            {
                //Debug.Log("next5");
                PlatformSix.nextPlatform = true;
                whichPlatform++;
            }
            else if (PlatformFour.nextPlatform && whichPlatform == 4)
            {
                //Debug.Log("next4");
                PlatformFive.nextPlatform = true;
                whichPlatform++;
            }
            else if (PlatformThree.nextPlatform && whichPlatform == 3)
            {
                //Debug.Log("next3");
                PlatformFour.nextPlatform = true;
                whichPlatform++;
                infopointTwo.SetActive(false);
            }
            else if (PlatformTwo.nextPlatform && whichPlatform == 2)
            {
                //Debug.Log("next2");
                PlatformThree.nextPlatform = true;
                whichPlatform++;
                
                infopointOne.SetActive(false);
                infopointThree.SetActive(true);
            }
            else if (PlatformOne.nextPlatform && whichPlatform == 1)
            {
                //Debug.Log("next1");
                PlatformTwo.nextPlatform = true;
                whichPlatform++;
                
                infopointTwo.SetActive(true);
            }
        }
    }
}
