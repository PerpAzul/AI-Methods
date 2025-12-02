using UnityEngine;
using UnityEngine.UI;

public class animationButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject nextScreen;
    public GameObject thisScreen;
    //public Button button;

    void Start()
    {
        //button.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            nextScreen.SetActive(true);
            thisScreen.SetActive(false);
        }
    }

    /*void TaskOnClick()
    {
        nextScreen.SetActive(true);
        thisScreen.SetActive(false);
    }*/
}
