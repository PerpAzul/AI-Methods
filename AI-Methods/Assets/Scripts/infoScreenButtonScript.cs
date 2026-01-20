using UnityEngine;
using UnityEngine.UI;

public class infoScreenButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject nextScreen;
    public GameObject thisScreen;
    public GameObject wholeInfopoint;
    public Button button;
    public bool finalScreen;

    void Start()
    {
        //button.onClick.AddListener(TaskOnClick);
        //thisScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if(finalScreen){
                //tiefensucheInfotafelScript.isReadingTurorial = false;
            //}

            if (!finalScreen)
            {
                nextScreen.SetActive(true);
            }
            wholeInfopoint.SetActive(false);
            thisScreen.SetActive(false);
        }
    }

    /*void TaskOnClick()
    {
        if (!finalScreen)
        {
            nextScreen.SetActive(true);
        }
        
        thisScreen.SetActive(false);
    }*/
}
