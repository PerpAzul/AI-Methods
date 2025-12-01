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
        button.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        if (!finalScreen)
        {
            nextScreen.SetActive(true);
        }
        
        thisScreen.SetActive(false);
    }
}
