using System;
using UnityEngine;
using UnityEngine.UI;

public class animationButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject nextScreen;
    public GameObject thisScreen;
    //public Button button;
    public Boolean finalScreen;

    void Start()
    {
        //button.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!finalScreen)
            {
                nextScreen.SetActive(true);
            }
            thisScreen.SetActive(false);
        }
    }

    /*void TaskOnClick()
    {
        nextScreen.SetActive(true);
        thisScreen.SetActive(false);
    }*/
}
