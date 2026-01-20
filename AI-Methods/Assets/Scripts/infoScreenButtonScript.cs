using UnityEngine;
using UnityEngine.UI;

public class infoScreenButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject nextScreen;
    public GameObject thisScreen;
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
            if (!finalScreen)
            {
                nextScreen.SetActive(true);
            }

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
